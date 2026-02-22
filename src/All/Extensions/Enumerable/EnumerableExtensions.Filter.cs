// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class EnumerableExtensions
{
	#region Maybe

	/// <summary>
	/// Filter out <see cref="None"/> objects from a list.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">List of Maybe objects.</param>
	/// <returns>List with all <see cref="None"/> objects removed.</returns>
	public static IEnumerable<Maybe<T>> Filter<T>(this IEnumerable<Maybe<T>> @this)
	{
		foreach (var item in @this)
		{
			foreach (var some in item)
			{
				yield return some;
			}
		}
	}

	/// <inheritdoc cref="Filter{T}(IEnumerable{Maybe{T}})"/>
	public static async Task<List<Maybe<T>>> FilterAsync<T>(this Task<IEnumerable<Maybe<T>>> @this)
	{
		var list = new List<Maybe<T>>();

		foreach (var item in await @this)
		{
			foreach (var some in item)
			{
				list.Add(some);
			}
		}

		return list;
	}

	/// <summary>
	/// Run <paramref name="fTest"/> on each value in <paramref name="this"/> that is <see cref="Some{T}"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">List of Maybe objects.</param>
	/// <param name="fTest">Function to detemine whether or not the value of <paramref name="this"/> should be returned.</param>
	/// <returns>Value of <paramref name="this"/> if <paramref name="fTest"/> returns true, or <see cref="None"/>.</returns>
	public static IEnumerable<Maybe<T>> Filter<T>(this IEnumerable<Maybe<T>> @this, Func<T, bool> fTest)
	{
		foreach (var item in @this)
		{
			foreach (var value in item.Filter(fTest))
			{
				yield return value;
			}
		}
	}

	/// <inheritdoc cref="Filter{T}(IEnumerable{Maybe{T}}, Func{T, bool})"/>
	public static Task<List<Maybe<T>>> FilterAsync<T>(this IEnumerable<Maybe<T>> @this, Func<T, Task<bool>> fTest) =>
		FilterAsync(Task.FromResult(@this), fTest);

	/// <inheritdoc cref="Filter{T}(IEnumerable{Maybe{T}}, Func{T, bool})"/>
	public static Task<List<Maybe<T>>> FilterAsync<T>(this Task<IEnumerable<Maybe<T>>> @this, Func<T, bool> fTest) =>
		FilterAsync(@this, async x => fTest(x));

	/// <inheritdoc cref="Filter{T}(IEnumerable{Maybe{T}}, Func{T, bool})"/>
	public static async Task<List<Maybe<T>>> FilterAsync<T>(this Task<IEnumerable<Maybe<T>>> @this, Func<T, Task<bool>> fTest)
	{
		var list = new List<Maybe<T>>();

		foreach (var item in await @this)
		{
			foreach (var value in await item.FilterAsync(fTest))
			{
				list.Add(value);
			}
		}

		return list;
	}

	#endregion

	#region Result

	/// <summary>
	/// Run <paramref name="fTest"/> on each value in <paramref name="this"/> that is <see cref="Some{T}"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">List of Result objects.</param>
	/// <param name="fTest">Function to detemine whether or not the value of <paramref name="this"/> should be returned.</param>
	/// <returns>Value of <paramref name="this"/> if <paramref name="fTest"/> returns true, or <see cref="None"/>.</returns>
	public static IEnumerable<Result<T>> Filter<T>(this IEnumerable<Result<T>> @this, Func<T, bool> fTest)
	{
		foreach (var item in @this)
		{
			foreach (var value in item.Filter(fTest).Unsafe())
			{
				yield return value;
			}
		}
	}

	/// <inheritdoc cref="Filter{T}(IEnumerable{Result{T}}, Func{T, bool})"/>
	public static Task<List<Result<T>>> FilterAsync<T>(this IEnumerable<Result<T>> @this, Func<T, Task<bool>> fTest) =>
		FilterAsync(Task.FromResult(@this), fTest);

	/// <inheritdoc cref="Filter{T}(IEnumerable{Result{T}}, Func{T, bool})"/>
	public static Task<List<Result<T>>> FilterAsync<T>(this Task<IEnumerable<Result<T>>> @this, Func<T, bool> fTest) =>
		FilterAsync(@this, async x => fTest(x));

	/// <inheritdoc cref="Filter{T}(IEnumerable{Result{T}}, Func{T, bool})"/>
	public static async Task<List<Result<T>>> FilterAsync<T>(this Task<IEnumerable<Result<T>>> @this, Func<T, Task<bool>> fTest)
	{
		var list = new List<Result<T>>();

		foreach (var item in await @this)
		{
			foreach (var value in await item.FilterAsync(fTest).Unsafe())
			{
				list.Add(value);
			}
		}

		return list;
	}

	#endregion
}
