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
	/// Run <paramref name="f"/> on the value of each item in <paramref name="this"/> that is <see cref="Some{T}"/>.
	/// </summary>
	/// <remarks>
	/// <see cref="None"/> items are kept in place. The result is lazy; the async overloads build the whole list.
	/// </remarks>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">List of Maybe objects.</param>
	/// <param name="f">Function that converts each value to a <see cref="Maybe{T}"/>.</param>
	/// <returns>One item for each item in <paramref name="this"/>: the <see cref="Maybe{T}"/> returned by <paramref name="f"/>, or <see cref="None"/>.</returns>
	public static IEnumerable<Maybe<TReturn>> Bind<T, TReturn>(this IEnumerable<Maybe<T>> @this, Func<T, Maybe<TReturn>> f)
	{
		foreach (var item in @this)
		{
			yield return item.Bind(f);
		}
	}

	/// <inheritdoc cref="Bind{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, Maybe{TReturn}})"/>
	public static async Task<IEnumerable<Maybe<TReturn>>> BindAsync<T, TReturn>(this IEnumerable<Maybe<T>> @this, Func<T, Task<Maybe<TReturn>>> f)
	{
		var items = new List<Maybe<TReturn>>();

		foreach (var item in @this)
		{
			items.Add(await item.BindAsync(f));
		}

		return items;
	}

	/// <inheritdoc cref="Bind{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, Maybe{TReturn}})"/>
	public static Task<IEnumerable<Maybe<TReturn>>> BindAsync<T, TReturn>(this Task<IEnumerable<Maybe<T>>> @this, Func<T, Maybe<TReturn>> f) =>
		@this.Then(x => x.Bind(f));

	/// <inheritdoc cref="Bind{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, Maybe{TReturn}})"/>
	public static Task<IEnumerable<Maybe<TReturn>>> BindAsync<T, TReturn>(this Task<IEnumerable<Maybe<T>>> @this, Func<T, Task<Maybe<TReturn>>> f) =>
		@this.Then(x => x.BindAsync(f));

	#endregion

	#region Result

	/// <summary>
	/// Run <paramref name="f"/> on the value of each item in <paramref name="this"/> that is <see cref="Ok{T}"/>.
	/// </summary>
	/// <remarks>
	/// <see cref="Failure"/> items are kept in place. The result is lazy; the async overloads build the whole list.
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">List of Result objects.</param>
	/// <param name="f">Function that converts each value to a <see cref="Result{T}"/>.</param>
	/// <returns>One item for each item in <paramref name="this"/>: the <see cref="Result{T}"/> returned by <paramref name="f"/>, or the original <see cref="Failure"/>.</returns>
	public static IEnumerable<Result<TReturn>> Bind<T, TReturn>(this IEnumerable<Result<T>> @this, Func<T, Result<TReturn>> f)
	{
		foreach (var item in @this)
		{
			yield return item.Bind(f);
		}
	}

	/// <inheritdoc cref="Bind{T, TReturn}(IEnumerable{Result{T}}, Func{T, Result{TReturn}})"/>
	public static async Task<IEnumerable<Result<TReturn>>> BindAsync<T, TReturn>(this IEnumerable<Result<T>> @this, Func<T, Task<Result<TReturn>>> f)
	{
		var items = new List<Result<TReturn>>();

		foreach (var item in @this)
		{
			items.Add(await item.BindAsync(f));
		}

		return items;
	}

	/// <inheritdoc cref="Bind{T, TReturn}(IEnumerable{Result{T}}, Func{T, Result{TReturn}})"/>
	public static Task<IEnumerable<Result<TReturn>>> BindAsync<T, TReturn>(this Task<IEnumerable<Result<T>>> @this, Func<T, Result<TReturn>> f) =>
		@this.Then(x => x.Bind(f));

	/// <inheritdoc cref="Bind{T, TReturn}(IEnumerable{Result{T}}, Func{T, Result{TReturn}})"/>
	public static Task<IEnumerable<Result<TReturn>>> BindAsync<T, TReturn>(this Task<IEnumerable<Result<T>>> @this, Func<T, Task<Result<TReturn>>> f) =>
		@this.Then(x => x.BindAsync(f));

	#endregion
}
