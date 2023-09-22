// Monads: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monads;

public static partial class EnumerableExtensions
{
	/// <summary>
	/// Filter out <see cref="None"/> objects from a list.
	/// </summary>
	/// <typeparam name="T">Maybe value type.</typeparam>
	/// <param name="this">List of Maybe objects.</param>
	/// <returns>List with all <see cref="None"/> objects removed.</returns>
	public static IEnumerable<Maybe<T>> Filter<T>(this IEnumerable<Maybe<T>> @this)
	{
		foreach (var item in @this)
		{
			yield return item;
		}
	}

	/// <inheritdoc cref="Filter{T}(IEnumerable{Maybe{T}})"/>
	public static async IAsyncEnumerable<Maybe<T>> FilterAsync<T>(this IAsyncEnumerable<Maybe<T>> @this)
	{
		await foreach (var item in @this)
		{
			yield return item;
		}
	}

	/// <summary>
	/// Run <paramref name="predicate"/> on each value in <paramref name="this"/> that is <see cref="Some{T}"/>.
	/// </summary>
	/// <typeparam name="T">Maybe value type.</typeparam>
	/// <param name="this">List of Maybe objects.</param>
	/// <param name="predicate">Function to detemine whether or not the value of <paramref name="this"/> should be returned.</param>
	/// <returns>Value of <paramref name="this"/> if <paramref name="predicate"/> returns true, or <see cref="None"/>.</returns>
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

	/// <inheritdoc cref="Filter{T}(IEnumerable{Maybe{T}}, Func{T, bool})"/>
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

	/// <inheritdoc cref="Filter{T}(IEnumerable{Maybe{T}}, Func{T, bool})"/>
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

	/// <inheritdoc cref="Filter{T}(IEnumerable{Maybe{T}}, Func{T, bool})"/>
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
