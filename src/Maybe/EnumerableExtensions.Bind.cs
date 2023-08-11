// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monadic;

public static partial class EnumerableExtensions
{
	public static IEnumerable<Maybe<TReturn>> Bind<T, TReturn>(this IEnumerable<Maybe<T>> @this, Func<T, Maybe<TReturn>> bind)
	{
		foreach (var item in @this)
		{
			foreach (var value in item)
			{
				if (value is not null)
				{
					yield return bind(value);
				}
			}
		}
	}

	public static async IAsyncEnumerable<Maybe<TReturn>> BindAsync<T, TReturn>(this IEnumerable<Maybe<T>> @this, Func<T, Task<Maybe<TReturn>>> bind)
	{
		foreach (var item in @this)
		{
			foreach (var value in item)
			{
				if (value is not null)
				{
					yield return await bind(value);
				}
			}
		}
	}

	public static IAsyncEnumerable<Maybe<TReturn>> BindAsync<T, TReturn>(this IAsyncEnumerable<Maybe<T>> @this, Func<T, Maybe<TReturn>> bind) =>
		BindAsync(@this, x => Task.FromResult(bind(x)));

	public static async IAsyncEnumerable<Maybe<TReturn>> BindAsync<T, TReturn>(this IAsyncEnumerable<Maybe<T>> @this, Func<T, Task<Maybe<TReturn>>> bind)
	{
		await foreach (var item in @this)
		{
			foreach (var value in item)
			{
				if (value is not null)
				{
					yield return await bind(value);
				}
			}
		}
	}
}
