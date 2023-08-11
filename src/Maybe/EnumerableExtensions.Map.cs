// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monadic;

public static partial class EnumerableExtensions
{
	public static IEnumerable<Maybe<TReturn>> Map<T, TReturn>(this IEnumerable<T> @this, Func<T, TReturn> map)
	{
		foreach (var item in @this)
		{
			if (item is not null && map(item) is TReturn value)
			{
				yield return value;
			}
		}
	}

	public static async IAsyncEnumerable<Maybe<TReturn>> MapAsync<T, TReturn>(this IEnumerable<T> @this, Func<T, Task<TReturn>> map)
	{
		foreach (var item in @this)
		{
			if (item is not null && await map(item) is TReturn value)
			{
				yield return value;
			}
		}
	}

	public static IAsyncEnumerable<Maybe<TReturn>> MapAsync<T, TReturn>(this IAsyncEnumerable<T> @this, Func<T, TReturn> map) =>
		MapAsync(@this, x => Task.FromResult(map(x)));

	public static async IAsyncEnumerable<Maybe<TReturn>> MapAsync<T, TReturn>(this IAsyncEnumerable<T> @this, Func<T, Task<TReturn>> map)
	{
		await foreach (var item in @this)
		{
			if (item is not null && await map(item) is TReturn value)
			{
				yield return value;
			}
		}
	}
}
