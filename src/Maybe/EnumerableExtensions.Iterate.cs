// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monadic;

public static partial class EnumerableExtensions
{
	/// <summary>
	/// Loop through list <paramref name="this"/> and apply function <paramref name="f"/> to each element
	/// that is a <see cref="Some{T}"/>.
	/// </summary>
	/// <typeparam name="T">Maybe value type.</typeparam>
	/// <param name="this">List of <see cref="Maybe{T}"/> objects.</param>
	/// <param name="f">Function to apply to each <see cref="Some{T}"/> element of <paramref name="this"/>.</param>
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

	/// <inheritdoc cref="Iterate{T}(IEnumerable{Maybe{T}}, Action{T})"/>
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

	/// <inheritdoc cref="Iterate{T}(IEnumerable{Maybe{T}}, Action{T})"/>
	public static async Task IterateAsync<T>(this IAsyncEnumerable<Maybe<T>> @this, Action<T> f)
	{
		await foreach (var item in @this)
		{
			foreach (var some in item)
			{
				f(some);
			}
		}
	}

	/// <inheritdoc cref="Iterate{T}(IEnumerable{Maybe{T}}, Action{T})"/>
	public static async Task IterateAsync<T>(this IAsyncEnumerable<Maybe<T>> @this, Func<T, Task> f)
	{
		await foreach (var item in @this)
		{
			foreach (var some in item)
			{
				await f(some);
			}
		}
	}
}
