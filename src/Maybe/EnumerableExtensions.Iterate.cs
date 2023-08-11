// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monadic;

public static partial class EnumerableExtensions
{
	public static void Iterate<T>(this IEnumerable<Maybe<T>> @this, Action<T> f)
	{
		foreach (var item in @this)
		{
			foreach (var some in item)
			{
				f(some);
			}
		}
	}

	public static async Task IterateAsync<T>(this IEnumerable<Maybe<T>> @this, Func<T, Task> f)
	{
		foreach (var item in @this)
		{
			foreach (var some in item)
			{
				await f(some);
			}
		}
	}
}
