// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Wrap.Caching;

/// <summary>
/// A simple cache over <see cref="IMemoryCache"/> that returns <see cref="Result{T}"/> objects, so a failure
/// always reaches the caller with its reason.
/// </summary>
/// <typeparam name="TKey">Key type.</typeparam>
public interface IWrapCache<TKey>
	where TKey : notnull
{
	/// <summary>
	/// Get a cache entry.
	/// </summary>
	/// <typeparam name="TValue">Value type.</typeparam>
	/// <param name="key">Entry key.</param>
	/// <returns>
	/// <see cref="Ok{T}"/> with the cached value, or a <see cref="Failure"/> if <paramref name="key"/> is
	/// <see langword="null"/>, there is no entry, or the entry is not a <typeparamref name="TValue"/>.
	/// </returns>
	Result<TValue> GetValue<TValue>(TKey key);

	/// <inheritdoc cref="SetValue{TValue}(TKey, TValue, MemoryCacheEntryOptions)"/>
	void SetValue<TValue>(TKey key, TValue value);

	/// <summary>
	/// Create or replace a cache entry.
	/// </summary>
	/// <typeparam name="TValue">Value type.</typeparam>
	/// <param name="key">Entry key.</param>
	/// <param name="value">Entry value.</param>
	/// <param name="opt">Entry options.</param>
	/// <exception cref="ArgumentNullException"><paramref name="key"/> or <paramref name="value"/> is <see langword="null"/>.</exception>
	void SetValue<TValue>(TKey key, TValue value, MemoryCacheEntryOptions opt);

	/// <inheritdoc cref="SetValueAsync{TValue}(TKey, Func{Task{TValue}}, MemoryCacheEntryOptions)"/>
	Task SetValueAsync<TValue>(TKey key, Func<Task<TValue>> valueFactory);

	/// <summary>
	/// Create or replace a cache entry with the value returned by <paramref name="valueFactory"/>.
	/// </summary>
	/// <typeparam name="TValue">Value type.</typeparam>
	/// <param name="key">Entry key.</param>
	/// <param name="valueFactory">Entry value factory.</param>
	/// <param name="opt">Entry options.</param>
	/// <exception cref="ArgumentNullException"><paramref name="key"/> or <paramref name="valueFactory"/> is <see langword="null"/>.</exception>
	Task SetValueAsync<TValue>(TKey key, Func<Task<TValue>> valueFactory, MemoryCacheEntryOptions opt);

	/// <inheritdoc cref="GetOrCreate{TValue}(TKey, Func{Result{TValue}}, MemoryCacheEntryOptions)"/>
	Result<TValue> GetOrCreate<TValue>(TKey key, Func<TValue> valueFactory);

	/// <inheritdoc cref="GetOrCreate{TValue}(TKey, Func{Result{TValue}}, MemoryCacheEntryOptions)"/>
	Result<TValue> GetOrCreate<TValue>(TKey key, Func<TValue> valueFactory, MemoryCacheEntryOptions opt);

	/// <inheritdoc cref="GetOrCreate{TValue}(TKey, Func{Result{TValue}}, MemoryCacheEntryOptions)"/>
	Result<TValue> GetOrCreate<TValue>(TKey key, Func<Result<TValue>> valueFactory);

	/// <summary>
	/// Get a cache entry, or create it with <paramref name="valueFactory"/> if there isn't one.
	/// </summary>
	/// <remarks>
	/// <para>
	/// A <see cref="Failure"/> from <paramref name="valueFactory"/> is returned as it is and nothing is stored,
	/// so the next call runs <paramref name="valueFactory"/> again.
	/// </para>
	/// <para>
	/// There is no locking: if calls for a missing key arrive together, each one runs
	/// <paramref name="valueFactory"/> and the last value stored wins. That suits factories that only read,
	/// for example a database query.
	/// </para>
	/// </remarks>
	/// <typeparam name="TValue">Value type.</typeparam>
	/// <param name="key">Entry key.</param>
	/// <param name="valueFactory">Entry value factory.</param>
	/// <param name="opt">Entry options.</param>
	/// <returns>
	/// <see cref="Ok{T}"/> with the cached or created value, or a <see cref="Failure"/> if <paramref name="key"/> is
	/// <see langword="null"/>, the entry is not a <typeparamref name="TValue"/>, or <paramref name="valueFactory"/>
	/// fails, throws or returns <see langword="null"/>.
	/// </returns>
	Result<TValue> GetOrCreate<TValue>(TKey key, Func<Result<TValue>> valueFactory, MemoryCacheEntryOptions opt);

	/// <inheritdoc cref="GetOrCreate{TValue}(TKey, Func{Result{TValue}}, MemoryCacheEntryOptions)"/>
	Task<Result<TValue>> GetOrCreateAsync<TValue>(TKey key, Func<Task<TValue>> valueFactory);

	/// <inheritdoc cref="GetOrCreate{TValue}(TKey, Func{Result{TValue}}, MemoryCacheEntryOptions)"/>
	Task<Result<TValue>> GetOrCreateAsync<TValue>(TKey key, Func<Task<TValue>> valueFactory, MemoryCacheEntryOptions opt);

	/// <inheritdoc cref="GetOrCreate{TValue}(TKey, Func{Result{TValue}}, MemoryCacheEntryOptions)"/>
	Task<Result<TValue>> GetOrCreateAsync<TValue>(TKey key, Func<Task<Result<TValue>>> valueFactory);

	/// <inheritdoc cref="GetOrCreate{TValue}(TKey, Func{Result{TValue}}, MemoryCacheEntryOptions)"/>
	Task<Result<TValue>> GetOrCreateAsync<TValue>(TKey key, Func<Task<Result<TValue>>> valueFactory, MemoryCacheEntryOptions opt);

	/// <summary>
	/// Remove a cache entry.
	/// </summary>
	/// <param name="key">Entry key.</param>
	/// <exception cref="ArgumentNullException"><paramref name="key"/> is <see langword="null"/>.</exception>
	void RemoveValue(TKey key);
}
