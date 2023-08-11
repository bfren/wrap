// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monadic;

public static partial class EnumerableExtensions
{
	public static IEnumerable<Maybe<T>> Filter<T>(this IEnumerable<Maybe<T>> @this) =>
		Filter(@this, x => true);

	public static IEnumerable<Maybe<T>> Filter<T>(this IEnumerable<Maybe<T>> @this, Func<T, bool> predicate)
	{
		foreach (var item in @this)
		{
			foreach (var some in item)
			{
				if (some is T value && predicate(value))
				{
					yield return value;
				}
			}
		}
	}

	public static IAsyncEnumerable<Maybe<T>> FilterAsync<T>(this IEnumerable<Maybe<T>> @this) =>
		FilterAsync(@this, x => Task.FromResult(true));

	public static async IAsyncEnumerable<Maybe<T>> FilterAsync<T>(this IEnumerable<Maybe<T>> @this, Func<T, Task<bool>> predicate)
	{
		foreach (var item in @this)
		{
			foreach (var some in item)
			{
				if (some is T value && await predicate(value))
				{
					yield return value;
				}
			}
		}
	}

	public static IAsyncEnumerable<Maybe<T>> FilterAsync<T>(this IAsyncEnumerable<Maybe<T>> @this) =>
		FilterAsync(@this, x => true);

	public static async IAsyncEnumerable<Maybe<T>> FilterAsync<T>(this IAsyncEnumerable<Maybe<T>> @this, Func<T, bool> predicate)
	{
		await foreach (var item in @this)
		{
			foreach (var some in item)
			{
				if (some is T value && predicate(value))
				{
					yield return value;
				}
			}
		}
	}

	public static async IAsyncEnumerable<Maybe<T>> FilterAsync<T>(this IAsyncEnumerable<Maybe<T>> @this, Func<T, Task<bool>> predicate)
	{
		await foreach (var item in @this)
		{
			foreach (var some in item)
			{
				if (some is T value && await predicate(value))
				{
					yield return value;
				}
			}
		}
	}
}
