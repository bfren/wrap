// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Wrap.Extensions;

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
	internal IMemoryCache Cache { get; private init; } = cache;

	// shortcut: 64 striped locks so unrelated keys rarely wait on each other - max 64 as held locks are tracked as bits in a ulong
	internal SemaphoreSlim[] CacheLocks { get; } = [.. Enumerable.Range(0, 64).Select(_ => new SemaphoreSlim(1, 1))];

	/// <inheritdoc/>
	public Maybe<Failure> LastFailure { get; private set; } = M.None;

	// one bit per lock held by this call chain, so a value factory that calls GetOrCreate for a key
	// sharing its lock runs without waiting on itself (SemaphoreSlim is not re-entrant)
	private readonly AsyncLocal<ulong> heldLocks = new();

	private int GetLockIndex(TKey key) =>
		(key.GetHashCode() & int.MaxValue) % CacheLocks.Length;

	private bool IsHeld(int index) =>
		(heldLocks.Value & (1UL << index)) != 0;

	private void SetHeld(int index, bool held) =>
		heldLocks.Value = held ? heldLocks.Value | (1UL << index) : heldLocks.Value & ~(1UL << index);

	/// <inheritdoc/>
	public Maybe<TValue> GetValue<TValue>(TKey key) =>
		GetValue<TValue>(key, out _);

	/// <summary>
	/// Get a value, returning any failure via <paramref name="failure"/> so concurrent callers
	/// never make decisions based on another caller's <see cref="LastFailure"/>.
	/// </summary>
	/// <typeparam name="TValue">Value type.</typeparam>
	/// <param name="key">Cache key.</param>
	/// <param name="failure">Set if the value could not be retrieved.</param>
	private Maybe<TValue> Get<TValue>(TKey key, out Maybe<Failure> failure)
	{
		failure = M.None;

		// Key cannot be null
		if (key is null)
		{
			failure = R.Fail("Key cannot be null.").Ctx(nameof(WrapCache), nameof(GetValue));
			return M.None;
		}

		// Attempt to get the value
		if (Cache.TryGetValue(key, out var value))
		{
			if (value is TValue correctType)
			{
				return correctType;
			}

			if (value is not null)
			{
				failure = R.Fail(
					"Requested cache entry '{Key}' is of type '{IncorrectType}' not the requested type '{RequestedType}'.",
					key, value.GetType().Name, typeof(TValue).Name
				).Ctx(nameof(WrapCache), nameof(GetValue));
			}
		}

		return M.None;
	}

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
	public Maybe<TValue> GetOrCreate<TValue>(TKey key, Func<TValue> valueFactory) =>
		GetOrCreate(key, () => M.Wrap(valueFactory()), new());

	/// <inheritdoc/>
	public Maybe<TValue> GetOrCreate<TValue>(TKey key, Func<TValue> valueFactory, MemoryCacheEntryOptions opt) =>
		GetOrCreate(key, () => M.Wrap(valueFactory()), opt);

	/// <inheritdoc/>
	public Maybe<TValue> GetOrCreate<TValue>(TKey key, Func<Maybe<TValue>> valueFactory) =>
		GetOrCreate(key, valueFactory, new());

	/// <inheritdoc/>
	public Maybe<TValue> GetOrCreate<TValue>(TKey key, Func<Maybe<TValue>> valueFactory, MemoryCacheEntryOptions opt)
	{
		// Check whether or not the value already exists, or there was an error
		var value = GetValue<TValue>(key, out var failure);
		if (value is Some<TValue> || failure is Some<Failure>)
		{
			return value;
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
			value = GetValue<TValue>(key, out failure);
			if (value is Some<TValue> || failure is Some<Failure>)
			{
				return value;
			}

			return valueFactory()
				.IfNone(
					() => LastFailure = R.Fail("Value factory returned null or None.")
						.Ctx(nameof(WrapCache), nameof(GetOrCreate))
				)
				.Map(
					x => Cache.GetOrCreate(key, e => { _ = e.SetOptions(opt).SetValue(x!); return x; })!
				);
		}
		catch (Exception ex)
		{
			// Return none on failure
			LastFailure = R.Fail(ex).Msg("Error creating cache value.")
				.Ctx(nameof(WrapCache), nameof(GetOrCreate));
			return M.None;
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
	public Task<Maybe<TValue>> GetOrCreateAsync<TValue>(TKey key, Func<Task<TValue>> valueFactory) =>
		GetOrCreateAsync(key, async () => M.Wrap(await valueFactory()), new());

	/// <inheritdoc/>
	public Task<Maybe<TValue>> GetOrCreateAsync<TValue>(TKey key, Func<Task<TValue>> valueFactory, MemoryCacheEntryOptions opt) =>
		GetOrCreateAsync(key, async () => M.Wrap(await valueFactory()), opt);

	/// <inheritdoc/>
	public Task<Maybe<TValue>> GetOrCreateAsync<TValue>(TKey key, Func<Task<Maybe<TValue>>> valueFactory) =>
		GetOrCreateAsync(key, valueFactory, new());

	/// <inheritdoc/>
	public async Task<Maybe<TValue>> GetOrCreateAsync<TValue>(TKey key, Func<Task<Maybe<TValue>>> valueFactory, MemoryCacheEntryOptions opt)
	{
		// Check whether or not the value already exists, or there was an error
		var value = GetValue<TValue>(key, out var failure);
		if (value is Some<TValue> || failure is Some<Failure>)
		{
			return value;
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
			value = GetValue<TValue>(key, out failure);
			if (value is Some<TValue> || failure is Some<Failure>)
			{
				return value;
			}

			return await valueFactory()
				.IfNoneAsync(
					() => LastFailure = R.Fail("Value factory returned null or None.")
						.Ctx(nameof(WrapCache), nameof(GetOrCreateAsync))
				)
				.MapAsync(
					x => Cache.GetOrCreate(key, e => { _ = e.SetOptions(opt).SetValue(x!); return x; })!
				);
		}
		catch (Exception ex)
		{
			// Return none on failure
			LastFailure = R.Fail(ex).Msg("Error creating cache value.")
				.Ctx(nameof(WrapCache), nameof(GetOrCreateAsync));
			return M.None;
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

	/// <summary>
	/// Wraps <see cref="Get{TValue}(TKey, out Maybe{Failure})"/> and also sets <see cref="LastFailure"/>
	/// for callers who read it.
	/// </summary>
	private Maybe<TValue> GetValue<TValue>(TKey key, out Maybe<Failure> failure)
	{
		var value = Get<TValue>(key, out failure);
		LastFailure = failure;
		return value;
	}
}
