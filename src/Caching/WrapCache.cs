// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Wrap.Caching;

/// <inheritdoc cref="IWrapCache{TKey}"/>
/// <param name="cache">Memory cache that stores the entries.</param>
// shortcut: no locking - calls for a missing key that arrive together each run their value factory and the last
// value stored wins; fine for factories that only read (e.g. database queries), add per-key de-duplication if one must run once
public sealed class WrapCache<TKey>(IMemoryCache cache) : IWrapCache<TKey>
	where TKey : notnull
{
	internal IMemoryCache Cache { get; private init; } =
		cache;

	private MemoryCacheEntryOptions DefaultOptions =>
		new() { Size = 1 };

	/// <inheritdoc/>
	public Result<TValue> GetValue<TValue>(TKey key) =>
		Find<TValue>(key, nameof(GetValue)) switch
		{
			Some<Result<TValue>> x =>
				x.Value,

			None =>
				Fail(nameof(GetValue), "Cache entry '{Key}' not found.", key)
		};

	/// <inheritdoc/>
	public void SetValue<TValue>(TKey key, TValue value) =>
		SetValue(key, value, DefaultOptions);

	/// <inheritdoc/>
	public void SetValue<TValue>(TKey key, TValue value, MemoryCacheEntryOptions options)
	{
		ArgumentNullException.ThrowIfNull(key);
		ArgumentNullException.ThrowIfNull(value);

		_ = Cache.Set(key, value, options);
	}

	/// <inheritdoc/>
	public Task SetValueAsync<TValue>(TKey key, Func<Task<TValue>> valueFactory) =>
		SetValueAsync(key, valueFactory, DefaultOptions);

	/// <inheritdoc/>
	public async Task SetValueAsync<TValue>(TKey key, Func<Task<TValue>> valueFactory, MemoryCacheEntryOptions options)
	{
		ArgumentNullException.ThrowIfNull(key);
		ArgumentNullException.ThrowIfNull(valueFactory);

		_ = Cache.Set(key, await valueFactory(), options);
	}

	/// <inheritdoc/>
	public Result<TValue> GetOrCreate<TValue>(TKey key, Func<TValue> valueFactory) =>
		GetOrCreate(key, () => R.Wrap(valueFactory()), DefaultOptions);

	/// <inheritdoc/>
	public Result<TValue> GetOrCreate<TValue>(TKey key, Func<TValue> valueFactory, MemoryCacheEntryOptions options) =>
		GetOrCreate(key, () => R.Wrap(valueFactory()), options);

	/// <inheritdoc/>
	public Result<TValue> GetOrCreate<TValue>(TKey key, Func<Result<TValue>> valueFactory) =>
		GetOrCreate(key, valueFactory, DefaultOptions);

	/// <inheritdoc/>
	public Result<TValue> GetOrCreate<TValue>(TKey key, Func<Result<TValue>> valueFactory, MemoryCacheEntryOptions options)
	{
		// Return the value if it exists, or the failure if the lookup failed
		if (Find<TValue>(key, nameof(GetOrCreate)) is Some<Result<TValue>> existing)
		{
			return existing.Value;
		}

		try
		{
			return Store(key, valueFactory(), options);
		}
		catch (Exception ex)
		{
			return R.Fail(ex).WithMessage("Error creating cache value.").WithContext(nameof(WrapCache<>), nameof(GetOrCreate));
		}
	}

	/// <inheritdoc/>
	public Task<Result<TValue>> GetOrCreateAsync<TValue>(TKey key, Func<Task<TValue>> valueFactory) =>
		GetOrCreateAsync(key, async () => R.Wrap(await valueFactory()), DefaultOptions);

	/// <inheritdoc/>
	public Task<Result<TValue>> GetOrCreateAsync<TValue>(TKey key, Func<Task<TValue>> valueFactory, MemoryCacheEntryOptions options) =>
		GetOrCreateAsync(key, async () => R.Wrap(await valueFactory()), options);

	/// <inheritdoc/>
	public Task<Result<TValue>> GetOrCreateAsync<TValue>(TKey key, Func<Task<Result<TValue>>> valueFactory) =>
		GetOrCreateAsync(key, valueFactory, DefaultOptions);

	/// <inheritdoc/>
	public async Task<Result<TValue>> GetOrCreateAsync<TValue>(TKey key, Func<Task<Result<TValue>>> valueFactory, MemoryCacheEntryOptions options)
	{
		// Return the value if it exists, or the failure if the lookup failed
		if (Find<TValue>(key, nameof(GetOrCreateAsync)) is Some<Result<TValue>> existing)
		{
			return existing.Value;
		}

		try
		{
			return Store(key, await valueFactory(), options);
		}
		catch (Exception ex)
		{
			return R.Fail(ex).WithMessage("Error creating cache value.").WithContext(nameof(WrapCache<>), nameof(GetOrCreateAsync));
		}
	}

	/// <inheritdoc/>
	public void RemoveValue(TKey key)
	{
		ArgumentNullException.ThrowIfNull(key);

		Cache.Remove(key);
	}

	/// <summary>
	/// Look up a cache entry.
	/// </summary>
	/// <typeparam name="TValue">Value type.</typeparam>
	/// <param name="key">Entry key.</param>
	/// <param name="function">Calling function, used as the failure context.</param>
	/// <returns>
	/// <see cref="None"/> if there is no entry (or it holds <see langword="null"/>), otherwise <see cref="Some{T}"/>
	/// holding the value, or a <see cref="Failure"/> if <paramref name="key"/> is <see langword="null"/> or the entry is
	/// not a <typeparamref name="TValue"/>.
	/// </returns>
	private Maybe<Result<TValue>> Find<TValue>(TKey key, string function)
	{
		if (key is null)
		{
			return M.Wrap<Result<TValue>>(Fail(function, "Key cannot be null."));
		}

		if (!Cache.TryGetValue(key, out var value) || value is null)
		{
			return M.None;
		}

		return value switch
		{
			TValue x =>
				M.Wrap(R.Wrap(x)),

			_ =>
				M.Wrap<Result<TValue>>(Fail(function,
					"Requested cache entry '{Key}' is of type '{IncorrectType}' not the requested type '{RequestedType}'.",
					key, value.GetType().Name, typeof(TValue).Name
				))
		};
	}

	/// <summary>
	/// Store the value returned by a value factory.
	/// </summary>
	/// <typeparam name="TValue">Value type.</typeparam>
	/// <param name="key">Entry key.</param>
	/// <param name="value">Value returned by the value factory.</param>
	/// <param name="options">Entry options.</param>
	/// <returns>The stored value, or <paramref name="value"/> unchanged (and nothing stored) if it is a <see cref="Failure"/>.</returns>
	private Result<TValue> Store<TValue>(TKey key, Result<TValue> value, MemoryCacheEntryOptions options) =>
		value switch
		{
			Ok<TValue> x =>
				Cache.Set(key, x.Value, options),

			Failure x =>
				x
		};

	/// <summary>
	/// Create a <see cref="Failure"/> with the cache as its context.
	/// </summary>
	/// <param name="function">Calling function.</param>
	/// <param name="message">Failure message.</param>
	/// <param name="args">Failure message arguments.</param>
	/// <returns>Failure object.</returns>
	private static Failure Fail(string function, string message, params object?[] args) =>
		R.Fail(message, args).WithContext(nameof(WrapCache<>), function);
}
