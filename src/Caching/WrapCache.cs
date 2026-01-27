// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
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

	internal SemaphoreSlim CacheLock { get; } = new(1, 1);

	/// <inheritdoc/>
	public Maybe<Failure> LastFailure { get; private set; } = M.None;

	/// <inheritdoc/>
	public Maybe<TValue> GetValue<TValue>(TKey key)
	{
		// Reset last failure
		LastFailure = M.None;

		// Key cannot be null
		if (key is null)
		{
			return fail(nameof(GetValue),
				"Key cannot be null."
			);
		}

		// Attempt to get the value
		if (Cache.TryGetValue(key, out var value))
		{
			return value switch
			{
				TValue correctType =>
					correctType,

				{ } incorrectType =>
					fail(nameof(GetValue),
						"Requested cache entry '{Key}' is of type '{IncorrectType}' not the requested type '{RequestedType}'.",
						key, incorrectType.GetType().Name, typeof(TValue).Name
					),

				_ =>
					M.None
			};
		}

		return M.None;

		// Set LastFailure before returning None
		Maybe<TValue> fail(string function, string message, params object?[] args)
		{
			LastFailure = R.Fail(message, [.. args]).Ctx(nameof(WrapCache), function);
			return M.None;
		}
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
		GetOrCreate(key, valueFactory, new());

	/// <inheritdoc/>
	public Maybe<TValue> GetOrCreate<TValue>(TKey key, Func<TValue> valueFactory, MemoryCacheEntryOptions opt) =>
		GetOrCreate(key, () => M.Wrap(valueFactory()), opt);

	/// <inheritdoc/>
	public Maybe<TValue> GetOrCreate<TValue>(TKey key, Func<Maybe<TValue>> valueFactory) =>
		GetOrCreate(key, valueFactory, new());

	/// <inheritdoc/>
	public Maybe<TValue> GetOrCreate<TValue>(TKey key, Func<Maybe<TValue>> valueFactory, MemoryCacheEntryOptions opt) =>
		GetOrCreateAsync(key, () => Task.FromResult(valueFactory()), opt).Result;

	/// <inheritdoc/>
	public Task<Maybe<TValue>> GetOrCreateAsync<TValue>(TKey key, Func<Task<TValue>> valueFactory) =>
		GetOrCreateAsync(key, valueFactory, new());

	/// <inheritdoc/>
	public Task<Maybe<TValue>> GetOrCreateAsync<TValue>(TKey key, Func<Task<TValue>> valueFactory, MemoryCacheEntryOptions opt) =>
		GetOrCreateAsync(key, async () => M.Wrap(await valueFactory()), opt);

	/// <inheritdoc/>
	public Task<Maybe<TValue>> GetOrCreateAsync<TValue>(TKey key, Func<Task<Maybe<TValue>>> valueFactory) =>
		GetOrCreateAsync(key, valueFactory, new());

	/// <inheritdoc/>
	public async Task<Maybe<TValue>> GetOrCreateAsync<TValue>(TKey key, Func<Task<Maybe<TValue>>> valueFactory, MemoryCacheEntryOptions opt)
	{
		// Check whether or not the value already exists
		var value = GetValue<TValue>(key);
		if (value.IsSome)
		{
			return value;
		}

		// If there was an error, return None
		if (LastFailure.IsSome)
		{
			return M.None;
		}

		// Lock all threads
		await CacheLock.WaitAsync();
		try
		{
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
			_ = CacheLock.Release();
		}
	}

	/// <inheritdoc/>
	public void RemoveValue(TKey key)
	{
		ArgumentNullException.ThrowIfNull(key);

		Cache.Remove(key);
	}
}
