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
	/// Convert the value of each <see cref="Some{T}"/> item that passes <paramref name="fTest"/> with <paramref name="f"/>, keeping only <see cref="Some{T}"/> results.
	/// </summary>
	/// <remarks>
	/// The result is lazy; the async overloads build the whole list.
	/// </remarks>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">List of Maybe objects.</param>
	/// <param name="fTest">Test to run on each value.</param>
	/// <param name="f">Function that converts each value, run when <paramref name="fTest"/> returns <see langword="true"/>.</param>
	/// <returns>A <see cref="Some{T}"/> for each value returned by <paramref name="f"/> that is not <see langword="null"/>.</returns>
	public static IEnumerable<Maybe<TReturn>> FilterMap<T, TReturn>(this IEnumerable<Maybe<T>> @this,
		Func<T, bool> fTest,
		Func<T, TReturn> f
	)
	{
		foreach (var item in @this)
		{
			if (item.MapIf(fTest, f) is Some<TReturn>(var value))
			{
				yield return value;
			}
		}
	}

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, TReturn})"/>
	public static async Task<IEnumerable<Maybe<TReturn>>> FilterMapAsync<T, TReturn>(this IEnumerable<Maybe<T>> @this,
		Func<T, bool> fTest,
		Func<T, Task<TReturn>> f
	)
	{
		var list = new List<Maybe<TReturn>>();

		foreach (var item in @this)
		{
			if ((await item.MapIfAsync(fTest, f)) is Some<TReturn>(var value))
			{
				list.Add(value);
			}
		}

		return list;
	}

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, TReturn})"/>
	public static Task<IEnumerable<Maybe<TReturn>>> FilterMapAsync<T, TReturn>(this Task<IEnumerable<Maybe<T>>> @this,
		Func<T, bool> fTest,
		Func<T, TReturn> f
	) =>
		@this.Then(x => x.FilterMap(fTest, f));

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, TReturn})"/>
	public static Task<IEnumerable<Maybe<TReturn>>> FilterMapAsync<T, TReturn>(this Task<IEnumerable<Maybe<T>>> @this,
		Func<T, bool> fTest,
		Func<T, Task<TReturn>> f
	) =>
		@this.Then(x => x.FilterMapAsync(fTest, f));

	#endregion

	#region Result

	/// <summary>
	/// Convert the value of each <see cref="Ok{T}"/> item that passes <paramref name="fTest"/> with <paramref name="f"/>, keeping only <see cref="Ok{T}"/> results.
	/// </summary>
	/// <remarks>
	/// <paramref name="f"/> runs through <see cref="ResultExtensions.MapIf{T, TReturn}(Result{T}, Func{T, bool}, Func{T, TReturn})"/>,
	/// so an item whose <paramref name="f"/> throws is dropped. The result is lazy; the async overloads build the whole list.
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">List of Result objects.</param>
	/// <param name="fTest">Test to run on each value.</param>
	/// <param name="f">Function that converts each value, run when <paramref name="fTest"/> returns <see langword="true"/>.</param>
	/// <returns>An <see cref="Ok{T}"/> for each value returned by <paramref name="f"/>; every <see cref="Failure"/> is dropped.</returns>
	public static IEnumerable<Result<TReturn>> FilterMap<T, TReturn>(this IEnumerable<Result<T>> @this,
		Func<T, bool> fTest,
		Func<T, TReturn> f
	)
	{
		foreach (var item in @this)
		{
			if (item.MapIf(fTest, f) is Ok<TReturn>(var value))
			{
				yield return value;
			}
		}
	}

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{Result{T}}, Func{T, bool}, Func{T, TReturn})"/>
	public static async Task<IEnumerable<Result<TReturn>>> FilterMapAsync<T, TReturn>(this IEnumerable<Result<T>> @this,
		Func<T, bool> fTest,
		Func<T, Task<TReturn>> f
	)
	{
		var list = new List<Result<TReturn>>();

		foreach (var item in @this)
		{
			if ((await item.MapIfAsync(fTest, f)) is Ok<TReturn>(var value))
			{
				list.Add(value);
			}
		}

		return list;
	}

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{Result{T}}, Func{T, bool}, Func{T, TReturn})"/>
	public static Task<IEnumerable<Result<TReturn>>> FilterMapAsync<T, TReturn>(this Task<IEnumerable<Result<T>>> @this,
		Func<T, bool> fTest,
		Func<T, TReturn> f
	) =>
		@this.Then(x => x.FilterMap(fTest, f));

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{Result{T}}, Func{T, bool}, Func{T, TReturn})"/>
	public static Task<IEnumerable<Result<TReturn>>> FilterMapAsync<T, TReturn>(this Task<IEnumerable<Result<T>>> @this,
		Func<T, bool> fTest,
		Func<T, Task<TReturn>> f
	) =>
		@this.Then(x => x.FilterMapAsync(fTest, f));

	#endregion
}
