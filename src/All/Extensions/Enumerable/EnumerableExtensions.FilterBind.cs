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
	/// Run <paramref name="f"/> on the value of each <see cref="Some{T}"/> item that passes <paramref name="fTest"/>, keeping only <see cref="Some{T}"/> results.
	/// </summary>
	/// <remarks>
	/// The result is lazy; the async overloads build the whole list.
	/// </remarks>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">List of Maybe objects.</param>
	/// <param name="fTest">Test to run on each value.</param>
	/// <param name="f">Function that converts each value to a <see cref="Maybe{T}"/>, run when <paramref name="fTest"/> returns <see langword="true"/>.</param>
	/// <returns>The <see cref="Some{T}"/> results of <paramref name="f"/>; every <see cref="None"/> is dropped.</returns>
	public static IEnumerable<Maybe<TReturn>> FilterBind<T, TReturn>(this IEnumerable<Maybe<T>> @this,
		Func<T, bool> fTest,
		Func<T, Maybe<TReturn>> f
	)
	{
		foreach (var item in @this)
		{
			if (item.BindIf(fTest, f) is Some<TReturn>(var value))
			{
				yield return value;
			}
		}
	}

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, Maybe{TReturn}})"/>
	public static async Task<IEnumerable<Maybe<TReturn>>> FilterBindAsync<T, TReturn>(this IEnumerable<Maybe<T>> @this,
		Func<T, bool> fTest,
		Func<T, Task<Maybe<TReturn>>> f
	)
	{
		var items = new List<Maybe<TReturn>>();

		foreach (var item in @this)
		{
			if ((await item.BindIfAsync(fTest, f)) is Some<TReturn>(var value))
			{
				items.Add(value);
			}
		}

		return items;
	}

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, Maybe{TReturn}})"/>
	public static Task<IEnumerable<Maybe<TReturn>>> FilterBindAsync<T, TReturn>(this Task<IEnumerable<Maybe<T>>> @this,
		Func<T, bool> fTest,
		Func<T, Maybe<TReturn>> f
	) =>
		@this.Then(x => x.FilterBind(fTest, f));

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, Maybe{TReturn}})"/>
	public static Task<IEnumerable<Maybe<TReturn>>> FilterBindAsync<T, TReturn>(this Task<IEnumerable<Maybe<T>>> @this,
		Func<T, bool> fTest,
		Func<T, Task<Maybe<TReturn>>> f
	) =>
		@this.Then(x => x.FilterBindAsync(fTest, f));

	#endregion

	#region Result

	/// <summary>
	/// Run <paramref name="f"/> on the value of each <see cref="Ok{T}"/> item that passes <paramref name="fTest"/>, keeping only <see cref="Ok{T}"/> results.
	/// </summary>
	/// <remarks>
	/// The result is lazy; the async overloads build the whole list.
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">List of Result objects.</param>
	/// <param name="fTest">Test to run on each value.</param>
	/// <param name="f">Function that converts each value to a <see cref="Result{T}"/>, run when <paramref name="fTest"/> returns <see langword="true"/>.</param>
	/// <returns>The <see cref="Ok{T}"/> results of <paramref name="f"/>; every <see cref="Failure"/> is dropped.</returns>
	public static IEnumerable<Result<TReturn>> FilterBind<T, TReturn>(this IEnumerable<Result<T>> @this,
		Func<T, bool> fTest,
		Func<T, Result<TReturn>> f
	)
	{
		foreach (var item in @this)
		{
			if (item.BindIf(fTest, f) is Ok<TReturn>(var value))
			{
				yield return value;
			}
		}
	}

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Result{T}}, Func{T, bool}, Func{T, Result{TReturn}})"/>
	public static async Task<IEnumerable<Result<TReturn>>> FilterBindAsync<T, TReturn>(this IEnumerable<Result<T>> @this,
		Func<T, bool> fTest,
		Func<T, Task<Result<TReturn>>> f
	)
	{
		var items = new List<Result<TReturn>>();

		foreach (var item in @this)
		{
			if ((await item.BindIfAsync(fTest, f)) is Ok<TReturn>(var value))
			{
				items.Add(value);
			}
		}

		return items;
	}

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Result{T}}, Func{T, bool}, Func{T, Result{TReturn}})"/>
	public static Task<IEnumerable<Result<TReturn>>> FilterBindAsync<T, TReturn>(this Task<IEnumerable<Result<T>>> @this,
		Func<T, bool> fTest,
		Func<T, Result<TReturn>> f
	) =>
		@this.Then(x => x.FilterBind(fTest, f));

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Result{T}}, Func{T, bool}, Func{T, Result{TReturn}})"/>
	public static Task<IEnumerable<Result<TReturn>>> FilterBindAsync<T, TReturn>(this Task<IEnumerable<Result<T>>> @this,
		Func<T, bool> fTest,
		Func<T, Task<Result<TReturn>>> f
	) =>
		@this.Then(x => x.FilterBindAsync(fTest, f));

	#endregion
}
