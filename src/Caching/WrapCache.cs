// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Wrap.Caching;

/// <inheritdoc cref="IWrapCache{TKey}"/>
public abstract class WrapCache
{
	/// <summary>
	/// Internal creation only.
	/// </summary>
	private protected WrapCache() { }
}

/// <inheritdoc cref="IWrapCache{TKey}"/>
/// <param name="cache">IMemoryCache for storing data.</param>
public sealed class WrapCache<TKey>(IMemoryCache cache) : WrapCache, IWrapCache<TKey>
	where TKey : notnull
{
	internal IMemoryCache Cache { get; private init; } =
		cache;

	// shortcut: 64 striped locks so unrelated keys rarely wait on each other - max 64 as held locks are tracked as bits in a ulong
	internal SemaphoreSlim[] CacheLocks { get; } =
		[.. Enumerable.Range(0, 64).Select(_ => new SemaphoreSlim(1, 1))];

	// one bit per lock held by this call chain, so a value factory that calls GetOrCreate for a key
	// sharing its lock runs without waiting on itself (SemaphoreSlim is not re-entrant)
	private readonly AsyncLocal<ulong> heldLocks =
		new();

	private int GetLockIndex(TKey key) =>
		(key.GetHashCode() & int.MaxValue) % CacheLocks.Length;

	private bool IsHeld(int index) =>
		(heldLocks.Value & (1UL << index)) != 0;

	private void SetHeld(int index, bool held) =>
		heldLocks.Value = held ? heldLocks.Value | (1UL << index) : heldLocks.Value & ~(1UL << index);

	/// <inheritdoc/>
	public Result<TValue> GetValue<TValue>(TKey key) =>
		Get<TValue>(key) switch
		{
			Some<Result<TValue>> x =>
				x.Value,

			None =>
				R.Fail("Cache entry '{Key}' not found.", key).Ctx(nameof(WrapCache), nameof(GetValue))
		};

	/// <summary>
	/// Look up a cache entry.
	/// </summary>
	/// <typeparam name="TValue">Value type.</typeparam>
	/// <param name="key">Cache key.</param>
	/// <returns>
	/// <see cref="None"/> if there is no entry (or it holds <see langword="null"/>), otherwise
	/// <see cref="Some{T}"/> holding the value, or a <see cref="Failure"/> if <paramref name="key"/> is
	/// <see langword="null"/> or the entry is not a <typeparamref name="TValue"/>.
	/// </returns>
	private Maybe<Result<TValue>> Get<TValue>(TKey key)
	{
		// Key cannot be null
		if (key is null)
		{
			return M.Wrap<Result<TValue>>(R.Fail("Key cannot be null.").Ctx(nameof(WrapCache), nameof(GetValue)));
		}

		// Treat a missing entry and a null entry the same
		if (!Cache.TryGetValue(key, out var value) || value is null)
		{
			return M.None;
		}

		return value switch
		{
			TValue correctType =>
				M.Wrap(R.Wrap(correctType)),

			_ =>
				M.Wrap<Result<TValue>>(R.Fail(
					"Requested cache entry '{Key}' is of type '{IncorrectType}' not the requested type '{RequestedType}'.",
					key, value.GetType().Name, typeof(TValue).Name
				).Ctx(nameof(WrapCache), nameof(GetValue)))
		};
	}

	/// <summary>
	/// Store the value made by a value factory.
	/// </summary>
	/// <typeparam name="TValue">Value type.</typeparam>
	/// <param name="key">Cache key.</param>
	/// <param name="value">Value returned by the value factory.</param>
	/// <param name="opt">Entry options.</param>
	/// <param name="function">Calling function, used as the failure context.</param>
	/// <returns>The cached value, or a <see cref="Failure"/> if <paramref name="value"/> is <see cref="None"/>.</returns>
	private Result<TValue> Create<TValue>(TKey key, Maybe<TValue> value, MemoryCacheEntryOptions opt, string function) =>
		value switch
		{
			Some<TValue> x =>
				Cache.GetOrCreate(key, e => { _ = e.SetOptions(opt).SetValue(x.Value!); return x.Value; })!,

			None =>
				R.Fail("Value factory returned null or None.").Ctx(nameof(WrapCache), function)
		};

	/// <inheritdoc/>
	public void SetValue<TValue>(TKey key, TValue value) =>
		SetValue(key, value, new());

	/// <inheritdoc/>
	public void SetValue<TValue>(TKey key, TValue value, MemoryCacheEntryOptions opt)
	{
		ArgumentNullException.ThrowIfNull(key);
		ArgumentNullException.ThrowIfNull(value);

		_ = Cache.Set(key, value, opt);
	}

	/// <inheritdoc/>
	public Task SetValueAsync<TValue>(TKey key, Func<Task<TValue>> valueFactory) =>
		SetValueAsync(key, valueFactory, new());

	/// <inheritdoc/>
	public async Task SetValueAsync<TValue>(TKey key, Func<Task<TValue>> valueFactory, MemoryCacheEntryOptions opt)
	{
		ArgumentNullException.ThrowIfNull(key);
		ArgumentNullException.ThrowIfNull(valueFactory);

		_ = Cache.Set(key, await valueFactory(), opt);
	}

	/// <inheritdoc/>
	public Result<TValue> GetOrCreate<TValue>(TKey key, Func<TValue> valueFactory) =>
		GetOrCreate(key, () => M.Wrap(valueFactory()), new());

	/// <inheritdoc/>
	public Result<TValue> GetOrCreate<TValue>(TKey key, Func<TValue> valueFactory, MemoryCacheEntryOptions opt) =>
		GetOrCreate(key, () => M.Wrap(valueFactory()), opt);

	/// <inheritdoc/>
	public Result<TValue> GetOrCreate<TValue>(TKey key, Func<Maybe<TValue>> valueFactory) =>
		GetOrCreate(key, valueFactory, new());

	/// <inheritdoc/>
	public Result<TValue> GetOrCreate<TValue>(TKey key, Func<Maybe<TValue>> valueFactory, MemoryCacheEntryOptions opt)
	{
		// Return the value if it already exists, or the failure if there was an error
		if (Get<TValue>(key) is Some<Result<TValue>> existing)
		{
			return existing.Value;
		}

		// Lock threads using this key
		var index = GetLockIndex(key);
		var entered = !IsHeld(index);
		if (entered)
		{
			CacheLocks[index].Wait();
			SetHeld(index, true);
		}

		try
		{
			// Another thread may have created the value while this one was waiting
			if (Get<TValue>(key) is Some<Result<TValue>> created)
			{
				return created.Value;
			}

			return Create(key, valueFactory(), opt, nameof(GetOrCreate));
		}
		catch (Exception ex)
		{
			return R.Fail(ex).Msg("Error creating cache value.")
				.Ctx(nameof(WrapCache), nameof(GetOrCreate));
		}
		finally
		{
			// Release other threads
			if (entered)
			{
				SetHeld(index, false);
				_ = CacheLocks[index].Release();
			}
		}
	}

	/// <inheritdoc/>
	public Task<Result<TValue>> GetOrCreateAsync<TValue>(TKey key, Func<Task<TValue>> valueFactory) =>
		GetOrCreateAsync(key, async () => M.Wrap(await valueFactory()), new());

	/// <inheritdoc/>
	public Task<Result<TValue>> GetOrCreateAsync<TValue>(TKey key, Func<Task<TValue>> valueFactory, MemoryCacheEntryOptions opt) =>
		GetOrCreateAsync(key, async () => M.Wrap(await valueFactory()), opt);

	/// <inheritdoc/>
	public Task<Result<TValue>> GetOrCreateAsync<TValue>(TKey key, Func<Task<Maybe<TValue>>> valueFactory) =>
		GetOrCreateAsync(key, valueFactory, new());

	/// <inheritdoc/>
	public async Task<Result<TValue>> GetOrCreateAsync<TValue>(TKey key, Func<Task<Maybe<TValue>>> valueFactory, MemoryCacheEntryOptions opt)
	{
		// Return the value if it already exists, or the failure if there was an error
		if (Get<TValue>(key) is Some<Result<TValue>> existing)
		{
			return existing.Value;
		}

		// Lock threads using this key
		var index = GetLockIndex(key);
		var entered = !IsHeld(index);
		if (entered)
		{
			await CacheLocks[index].WaitAsync();
			SetHeld(index, true);
		}

		try
		{
			// Another thread may have created the value while this one was waiting
			if (Get<TValue>(key) is Some<Result<TValue>> created)
			{
				return created.Value;
			}

			return Create(key, await valueFactory(), opt, nameof(GetOrCreateAsync));
		}
		catch (Exception ex)
		{
			return R.Fail(ex).Msg("Error creating cache value.")
				.Ctx(nameof(WrapCache), nameof(GetOrCreateAsync));
		}
		finally
		{
			// Release other threads
			if (entered)
			{
				SetHeld(index, false);
				_ = CacheLocks[index].Release();
			}
		}
	}

	/// <inheritdoc/>
	public void RemoveValue(TKey key)
	{
		ArgumentNullException.ThrowIfNull(key);

		Cache.Remove(key);
	}
}
