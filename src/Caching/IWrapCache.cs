// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Wrap.Caching;

/// <summary>
/// Asynchronous and null-safe cache implementation.
/// </summary>
/// <typeparam name="TKey">Key type.</typeparam>
public interface IWrapCache<TKey>
	where TKey : notnull
{
	/// <summary>
	/// Retrieve a cache entry if it exists.
	/// </summary>
	/// <typeparam name="TValue">Value Type.</typeparam>
	/// <param name="key">Entry key.</param>
	/// <returns>
	/// <see cref="Ok{T}"/> with the cached value, or a <see cref="Failure"/> if <paramref name="key"/> is
	/// <see langword="null"/>, there is no entry, or the entry is not a <typeparamref name="TValue"/>.
	/// </returns>
	Result<TValue> GetValue<TValue>(TKey key);

	/// <inheritdoc cref="SetValue{TValue}(TKey, TValue, MemoryCacheEntryOptions)"/>
	void SetValue<TValue>(TKey key, TValue value);

	/// <summary>
	/// Create a cache entry with specified values.
	/// </summary>
	/// <typeparam name="TValue">Value Type.</typeparam>
	/// <param name="key">Entry key.</param>
	/// <param name="value">Entry value.</param>
	/// <param name="opt">Entry options.</param>
	void SetValue<TValue>(TKey key, TValue value, MemoryCacheEntryOptions opt);

	/// <inheritdoc cref="SetValue{TValue}(TKey, TValue, MemoryCacheEntryOptions)"/>
	Task SetValueAsync<TValue>(TKey key, Func<Task<TValue>> valueFactory);

	/// <inheritdoc cref="SetValue{TValue}(TKey, TValue, MemoryCacheEntryOptions)"/>
	Task SetValueAsync<TValue>(TKey key, Func<Task<TValue>> valueFactory, MemoryCacheEntryOptions opt);

	/// <inheritdoc cref="GetOrCreate{TValue}(TKey, Func{Maybe{TValue}}, MemoryCacheEntryOptions)"/>
	Result<TValue> GetOrCreate<TValue>(TKey key, Func<TValue> valueFactory);

	/// <inheritdoc cref="GetOrCreate{TValue}(TKey, Func{Maybe{TValue}}, MemoryCacheEntryOptions)"/>
	Result<TValue> GetOrCreate<TValue>(TKey key, Func<TValue> valueFactory, MemoryCacheEntryOptions opt);

	/// <inheritdoc cref="GetOrCreate{TValue}(TKey, Func{Maybe{TValue}}, MemoryCacheEntryOptions)"/>
	Result<TValue> GetOrCreate<TValue>(TKey key, Func<Maybe<TValue>> valueFactory);

	/// <summary>
	/// Get a value from the cache or create it if it doesn't exist.
	/// </summary>
	/// <remarks>
	/// Each call returns its own <see cref="Failure"/>, so concurrent callers never see each other's errors.
	/// </remarks>
	/// <typeparam name="TValue">Value Type.</typeparam>
	/// <param name="key">Entry key.</param>
	/// <param name="valueFactory">Entry value factory.</param>
	/// <param name="opt">Entry options.</param>
	/// <returns>
	/// <see cref="Ok{T}"/> with the cached or created value, or a <see cref="Failure"/> if <paramref name="key"/> is
	/// <see langword="null"/>, the entry is not a <typeparamref name="TValue"/>, or <paramref name="valueFactory"/>
	/// throws or returns <see langword="null"/> or <see cref="None"/>.
	/// </returns>
	Result<TValue> GetOrCreate<TValue>(TKey key, Func<Maybe<TValue>> valueFactory, MemoryCacheEntryOptions opt);

	/// <inheritdoc cref="GetOrCreate{TValue}(TKey, Func{Maybe{TValue}}, MemoryCacheEntryOptions)"/>
	Task<Result<TValue>> GetOrCreateAsync<TValue>(TKey key, Func<Task<TValue>> valueFactory);

	/// <inheritdoc cref="GetOrCreate{TValue}(TKey, Func{Maybe{TValue}}, MemoryCacheEntryOptions)"/>
	Task<Result<TValue>> GetOrCreateAsync<TValue>(TKey key, Func<Task<TValue>> valueFactory, MemoryCacheEntryOptions opt);

	/// <inheritdoc cref="GetOrCreate{TValue}(TKey, Func{Maybe{TValue}}, MemoryCacheEntryOptions)"/>
	Task<Result<TValue>> GetOrCreateAsync<TValue>(TKey key, Func<Task<Maybe<TValue>>> valueFactory);

	/// <inheritdoc cref="GetOrCreate{TValue}(TKey, Func{Maybe{TValue}}, MemoryCacheEntryOptions)"/>
	Task<Result<TValue>> GetOrCreateAsync<TValue>(TKey key, Func<Task<Maybe<TValue>>> valueFactory, MemoryCacheEntryOptions opt);

	/// <summary>
	/// Remove a value from the cache.
	/// </summary>
	/// <param name="key">Entry key.</param>
	void RemoveValue(TKey key);
}
