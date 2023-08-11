// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monadic;

public static partial class EnumerableExtensions
{
	public static IEnumerable<Maybe<TReturn>> FilterBind<T, TReturn>(this IEnumerable<Maybe<T>> @this,
		Func<T, bool> predicate,
		Func<T, Maybe<TReturn>> bind
	)
	{
		foreach (var item in @this)
		{
			foreach (var some in item)
			{
				if (some is T value && predicate(value))
				{
					yield return bind(value);
				}
			}
		}
	}

	public static IAsyncEnumerable<Maybe<TReturn>> FilterBindAsync<T, TReturn>(this IEnumerable<Maybe<T>> @this,
		Func<T, bool> predicate,
		Func<T, Task<Maybe<TReturn>>> bind
	) =>
		FilterBindAsync(@this, x => Task.FromResult(predicate(x)), bind);

	public static IAsyncEnumerable<Maybe<TReturn>> FilterBindAsync<T, TReturn>(this IEnumerable<Maybe<T>> @this,
		Func<T, Task<bool>> predicate,
		Func<T, Maybe<TReturn>> bind
	) =>
		FilterBindAsync(@this, predicate, x => Task.FromResult(bind(x)));

	public static async IAsyncEnumerable<Maybe<TReturn>> FilterBindAsync<T, TReturn>(this IEnumerable<Maybe<T>> @this,
		Func<T, Task<bool>> predicate,
		Func<T, Task<Maybe<TReturn>>> bind
	)
	{
		foreach (var item in @this)
		{
			foreach (var some in item)
			{
				if (some is T value && await predicate(value))
				{
					yield return await bind(value);
				}
			}
		}
	}

	public static IAsyncEnumerable<Maybe<TReturn>> FilterBindAsync<T, TReturn>(this IAsyncEnumerable<Maybe<T>> @this,
		Func<T, bool> predicate,
		Func<T, Task<Maybe<TReturn>>> bind
	) =>
		FilterBindAsync(@this, x => Task.FromResult(predicate(x)), bind);

	public static IAsyncEnumerable<Maybe<TReturn>> FilterBindAsync<T, TReturn>(this IAsyncEnumerable<Maybe<T>> @this,
		Func<T, Task<bool>> predicate,
		Func<T, Maybe<TReturn>> bind
	) =>
		FilterBindAsync(@this, predicate, x => Task.FromResult(bind(x)));

	public static async IAsyncEnumerable<Maybe<TReturn>> FilterBindAsync<T, TReturn>(this IAsyncEnumerable<Maybe<T>> @this,
		Func<T, Task<bool>> predicate,
		Func<T, Task<Maybe<TReturn>>> bind
	)
	{
		await foreach (var item in @this)
		{
			foreach (var some in item)
			{
				if (some is T value && await predicate(value))
				{
					yield return await bind(value);
				}
			}
		}
	}
}
