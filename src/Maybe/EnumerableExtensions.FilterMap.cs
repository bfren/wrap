// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wrap;

public static partial class EnumerableExtensions
{
	/// <summary>
	/// Run <paramref name="map"/> on each element of <paramref name="this"/> that matches <paramref name="predicate"/>.
	/// </summary>
	/// <typeparam name="T">Maybe value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">List of Maybe objects.</param>
	/// <param name="predicate">Function to detemine whether or not the value of <paramref name="this"/> should be passed to <paramref name="map"/>.</param>
	/// <param name="map">Function to convert a <typeparamref name="T"/> object to a <typeparamref name="TReturn"/> object.</param>
	/// <returns>List of <see cref="Maybe{T}"/> objects returned by <paramref name="map"/>.</returns>
	public static IEnumerable<Maybe<TReturn>> FilterMap<T, TReturn>(this IEnumerable<Maybe<T>> @this,
		Func<T, bool> predicate,
		Func<T, TReturn> map
	)
	{
		foreach (var item in @this)
		{
			foreach (var some in item)
			{
				if (some is T value && predicate(value))
				{
					yield return map(value);
				}
			}
		}
	}

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, TReturn})"/>
	public static IAsyncEnumerable<Maybe<TReturn>> FilterMapAsync<T, TReturn>(this IEnumerable<Maybe<T>> @this,
		Func<T, bool> predicate,
		Func<T, Task<TReturn>> map
	) =>
		FilterMapAsync(@this, x => Task.FromResult(predicate(x)), map);

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, TReturn})"/>
	public static IAsyncEnumerable<Maybe<TReturn>> FilterMapAsync<T, TReturn>(this IEnumerable<Maybe<T>> @this,
		Func<T, Task<bool>> predicate,
		Func<T, TReturn> map
	) =>
		FilterMapAsync(@this, predicate, x => Task.FromResult(map(x)));

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, TReturn})"/>
	public static async IAsyncEnumerable<Maybe<TReturn>> FilterMapAsync<T, TReturn>(this IEnumerable<Maybe<T>> @this,
		Func<T, Task<bool>> predicate,
		Func<T, Task<TReturn>> map
	)
	{
		foreach (var item in @this)
		{
			foreach (var some in item)
			{
				if (some is T value && await predicate(value))
				{
					yield return await map(value);
				}
			}
		}
	}

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, TReturn})"/>
	public static IAsyncEnumerable<Maybe<TReturn>> FilterMapAsync<T, TReturn>(this IAsyncEnumerable<Maybe<T>> @this,
		Func<T, bool> predicate,
		Func<T, Task<TReturn>> map
	) =>
		FilterMapAsync(@this, x => Task.FromResult(predicate(x)), map);

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, TReturn})"/>
	public static IAsyncEnumerable<Maybe<TReturn>> FilterMapAsync<T, TReturn>(this IAsyncEnumerable<Maybe<T>> @this,
		Func<T, Task<bool>> predicate,
		Func<T, TReturn> map
	) =>
		FilterMapAsync(@this, predicate, x => Task.FromResult(map(x)));

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, TReturn})"/>
	public static async IAsyncEnumerable<Maybe<TReturn>> FilterMapAsync<T, TReturn>(this IAsyncEnumerable<Maybe<T>> @this,
		Func<T, Task<bool>> predicate,
		Func<T, Task<TReturn>> map
	)
	{
		await foreach (var item in @this)
		{
			foreach (var some in item)
			{
				if (some is T value && await predicate(value))
				{
					yield return await map(value);
				}
			}
		}
	}
}
