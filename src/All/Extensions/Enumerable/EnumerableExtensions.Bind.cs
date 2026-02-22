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
	/// Run <paramref name="f"/> on each value in <paramref name="this"/> that is <see cref="Some{T}"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">List of Maybe objects.</param>
	/// <param name="f">Function to convert a <typeparamref name="T"/> object to a <typeparamref name="TReturn"/> object.</param>
	/// <returns>List of <see cref="Maybe{T}"/> objects returned by <paramref name="f"/>.</returns>
	public static IEnumerable<Maybe<TReturn>> Bind<T, TReturn>(this IEnumerable<Maybe<T>> @this, Func<T, Maybe<TReturn>> f)
	{
		foreach (var item in @this.Filter())
		{
			yield return item.Bind(f);
		}
	}

	/// <inheritdoc cref="Bind{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, Maybe{TReturn}})"/>
	public static Task<List<Maybe<TReturn>>> BindAsync<T, TReturn>(this IEnumerable<Maybe<T>> @this, Func<T, Task<Maybe<TReturn>>> f) =>
		BindAsync(Task.FromResult(@this), f);

	/// <inheritdoc cref="Bind{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, Maybe{TReturn}})"/>
	public static Task<List<Maybe<TReturn>>> BindAsync<T, TReturn>(this Task<IEnumerable<Maybe<T>>> @this, Func<T, Maybe<TReturn>> f) =>
		BindAsync(@this, async x => f(x));

	/// <inheritdoc cref="Bind{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, Maybe{TReturn}})"/>
	public static async Task<List<Maybe<TReturn>>> BindAsync<T, TReturn>(this Task<IEnumerable<Maybe<T>>> @this, Func<T, Task<Maybe<TReturn>>> f)
	{
		var items = new List<Maybe<TReturn>>();

		foreach (var item in await @this.FilterAsync())
		{
			items.Add(await item.BindAsync(f));
		}

		return items;
	}

	#endregion

	#region Result

	/// <summary>
	/// Run <paramref name="f"/> on each value in <paramref name="this"/> that is <see cref="Ok{T}"/>.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">List of Resultobjects.</param>
	/// <param name="f">Function to convert a <typeparamref name="T"/> object to a <typeparamref name="TReturn"/> object.</param>
	/// <returns>List of <see cref="Result{T}"/> objects returned by <paramref name="f"/>.</returns>
	public static IEnumerable<Result<TReturn>> Bind<T, TReturn>(this IEnumerable<Result<T>> @this, Func<T, Result<TReturn>> f)
	{
		foreach (var item in @this)
		{
			yield return item.Bind(f);
		}
	}

	/// <inheritdoc cref="Bind{T, TReturn}(IEnumerable{Result{T}}, Func{T, Result{TReturn}})"/>
	public static Task<List<Result<TReturn>>> BindAsync<T, TReturn>(this IEnumerable<Result<T>> @this, Func<T, Task<Result<TReturn>>> f) =>
		BindAsync(Task.FromResult(@this), f);

	/// <inheritdoc cref="Bind{T, TReturn}(IEnumerable{Result{T}}, Func{T, Result{TReturn}})"/>
	public static Task<List<Result<TReturn>>> BindAsync<T, TReturn>(this Task<IEnumerable<Result<T>>> @this, Func<T, Result<TReturn>> f) =>
		BindAsync(@this, async x => f(x));

	/// <inheritdoc cref="Bind{T, TReturn}(IEnumerable{Result{T}}, Func{T, Result{TReturn}})"/>
	public static async Task<List<Result<TReturn>>> BindAsync<T, TReturn>(this Task<IEnumerable<Result<T>>> @this, Func<T, Task<Result<TReturn>>> f)
	{
		var items = new List<Result<TReturn>>();

		foreach (var item in await @this)
		{
			items.Add(await item.BindAsync(f));
		}

		return items;
	}

	#endregion
}
