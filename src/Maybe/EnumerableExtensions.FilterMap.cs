// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monadic;

public static partial class EnumerableExtensions
{
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

	public static IAsyncEnumerable<Maybe<TReturn>> FilterMapAsync<T, TReturn>(this IEnumerable<Maybe<T>> @this,
		Func<T, bool> predicate,
		Func<T, Task<TReturn>> map
	) =>
		FilterMapAsync(@this, x => Task.FromResult(predicate(x)), map);

	public static IAsyncEnumerable<Maybe<TReturn>> FilterMapAsync<T, TReturn>(this IEnumerable<Maybe<T>> @this,
		Func<T, Task<bool>> predicate,
		Func<T, TReturn> map
	) =>
		FilterMapAsync(@this, predicate, x => Task.FromResult(map(x)));

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

	public static IAsyncEnumerable<Maybe<TReturn>> FilterMapAsync<T, TReturn>(this IAsyncEnumerable<Maybe<T>> @this,
		Func<T, bool> predicate,
		Func<T, Task<TReturn>> map
	) =>
		FilterMapAsync(@this, x => Task.FromResult(predicate(x)), map);

	public static IAsyncEnumerable<Maybe<TReturn>> FilterMapAsync<T, TReturn>(this IAsyncEnumerable<Maybe<T>> @this,
		Func<T, Task<bool>> predicate,
		Func<T, TReturn> map
	) =>
		FilterMapAsync(@this, predicate, x => Task.FromResult(map(x)));

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
