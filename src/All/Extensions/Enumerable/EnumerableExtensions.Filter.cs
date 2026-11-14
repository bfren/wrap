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
	/// Remove the <see cref="None"/> items from a list.
	/// </summary>
	/// <remarks>
	/// The result is lazy.
	/// </remarks>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">List of Maybe objects.</param>
	/// <returns>The <see cref="Some{T}"/> items of <paramref name="this"/>, in order.</returns>
	public static IEnumerable<Maybe<T>> Filter<T>(this IEnumerable<Maybe<T>> @this)
	{
		foreach (var item in @this)
		{
			if (item is Some<T>(var some))
			{
				yield return some;
			}
		}
	}

	/// <inheritdoc cref="Filter{T}(IEnumerable{Maybe{T}})"/>
	public static Task<IEnumerable<Maybe<T>>> FilterAsync<T>(this Task<IEnumerable<Maybe<T>>> @this) =>
		@this.Then(x => x.Filter());

	/// <summary>
	/// Keep the <see cref="Some{T}"/> items whose value passes <paramref name="fTest"/>.
	/// </summary>
	/// <remarks>
	/// The result is lazy; the async overloads build the whole list.
	/// </remarks>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">List of Maybe objects.</param>
	/// <param name="fTest">Test to run on each value.</param>
	/// <returns>The <see cref="Some{T}"/> items for which <paramref name="fTest"/> returns <see langword="true"/>; <see cref="None"/> items are dropped.</returns>
	public static IEnumerable<Maybe<T>> Filter<T>(this IEnumerable<Maybe<T>> @this, Func<T, bool> fTest)
	{
		foreach (var item in @this)
		{
			if (item.Filter(fTest) is Some<T>(var value))
			{
				yield return value;
			}
		}
	}

	/// <inheritdoc cref="Filter{T}(IEnumerable{Maybe{T}}, Func{T, bool})"/>
	public static async Task<IEnumerable<Maybe<T>>> FilterAsync<T>(this IEnumerable<Maybe<T>> @this, Func<T, Task<bool>> fTest)
	{
		var list = new List<Maybe<T>>();

		foreach (var item in @this)
		{
			if ((await item.FilterAsync(fTest)) is Some<T>(var value))
			{
				list.Add(value);
			}
		}

		return list;
	}

	/// <inheritdoc cref="Filter{T}(IEnumerable{Maybe{T}}, Func{T, bool})"/>
	public static Task<IEnumerable<Maybe<T>>> FilterAsync<T>(this Task<IEnumerable<Maybe<T>>> @this, Func<T, bool> fTest) =>
		@this.Then(x => x.Filter(fTest));

	/// <inheritdoc cref="Filter{T}(IEnumerable{Maybe{T}}, Func{T, bool})"/>
	public static Task<IEnumerable<Maybe<T>>> FilterAsync<T>(this Task<IEnumerable<Maybe<T>>> @this, Func<T, Task<bool>> fTest) =>
		@this.Then(x => x.FilterAsync(fTest));

	#endregion

	#region Result

	/// <summary>
	/// Keep the <see cref="Ok{T}"/> items whose value passes <paramref name="fTest"/>.
	/// </summary>
	/// <remarks>
	/// The result is lazy; the async overloads build the whole list.
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">List of Result objects.</param>
	/// <param name="fTest">Test to run on each value.</param>
	/// <returns>The <see cref="Ok{T}"/> items for which <paramref name="fTest"/> returns <see langword="true"/>; <see cref="Failure"/> items are dropped.</returns>
	public static IEnumerable<Result<T>> Filter<T>(this IEnumerable<Result<T>> @this, Func<T, bool> fTest)
	{
		foreach (var item in @this)
		{
			if (item.Filter(fTest) is Ok<T>(var value))
			{
				yield return value;
			}
		}
	}

	/// <inheritdoc cref="Filter{T}(IEnumerable{Result{T}}, Func{T, bool})"/>
	public static async Task<IEnumerable<Result<T>>> FilterAsync<T>(this IEnumerable<Result<T>> @this, Func<T, Task<bool>> fTest)
	{
		var list = new List<Result<T>>();

		foreach (var item in @this)
		{
			if ((await item.FilterAsync(fTest)) is Ok<T>(var value))
			{
				list.Add(value);
			}
		}

		return list;
	}

	/// <inheritdoc cref="Filter{T}(IEnumerable{Result{T}}, Func{T, bool})"/>
	public static Task<IEnumerable<Result<T>>> FilterAsync<T>(this Task<IEnumerable<Result<T>>> @this, Func<T, bool> fTest) =>
		@this.Then(x => x.Filter(fTest));

	/// <inheritdoc cref="Filter{T}(IEnumerable{Result{T}}, Func{T, bool})"/>
	public static Task<IEnumerable<Result<T>>> FilterAsync<T>(this Task<IEnumerable<Result<T>>> @this, Func<T, Task<bool>> fTest) =>
		@this.Then(x => x.FilterAsync(fTest));

	#endregion
}
