// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class EnumerableExtensions
{
	#region Maybe

	/// <summary>
	/// Run <paramref name="f"/> on each element of <paramref name="this"/> that matches <paramref name="fTest"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">List of Maybe objects.</param>
	/// <param name="fTest">Function to detemine whether or not the value of <paramref name="this"/> should be passed to <paramref name="f"/>.</param>
	/// <param name="f">Function to convert a <typeparamref name="T"/> object to a <typeparamref name="TReturn"/> object.</param>
	/// <returns>List of <see cref="Maybe{T}"/> objects returned by <paramref name="f"/>.</returns>
	public static IEnumerable<Maybe<TReturn>> FilterBind<T, TReturn>(this IEnumerable<Maybe<T>> @this,
		Func<T, bool> fTest,
		Func<T, Maybe<TReturn>> f
	)
	{
		foreach (var item in @this.Filter())
		{
			foreach (var value in item.BindIf(fTest, f))
			{
				yield return value;
			}
		}
	}

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, Maybe{TReturn}})"/>
	public static Task<List<Maybe<TReturn>>> FilterBindAsync<T, TReturn>(this IEnumerable<Maybe<T>> @this,
		Func<T, bool> fTest,
		Func<T, Task<Maybe<TReturn>>> f
	) =>
		FilterBindAsync(Task.FromResult(@this), fTest, f);

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, Maybe{TReturn}})"/>
	public static Task<List<Maybe<TReturn>>> FilterBindAsync<T, TReturn>(this Task<IEnumerable<Maybe<T>>> @this,
		Func<T, bool> fTest,
		Func<T, Maybe<TReturn>> f
	) =>
		FilterBindAsync(@this, fTest, async x => f(x));

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, Maybe{TReturn}})"/>
	public static async Task<List<Maybe<TReturn>>> FilterBindAsync<T, TReturn>(this Task<IEnumerable<Maybe<T>>> @this,
		Func<T, bool> fTest,
		Func<T, Task<Maybe<TReturn>>> f
	)
	{
		var items = new List<Maybe<TReturn>>();

		foreach (var item in await @this.FilterAsync())
		{
			foreach (var value in await item.BindIfAsync(fTest, f))
			{
				items.Add(value);
			}
		}

		return items;
	}

	#endregion

	#region Result

	/// <summary>
	/// Run <paramref name="f"/> on each element of <paramref name="this"/> that matches <paramref name="fTest"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">List of Result objects.</param>
	/// <param name="fTest">Function to detemine whether or not the value of <paramref name="this"/> should be passed to <paramref name="f"/>.</param>
	/// <param name="f">Function to convert a <typeparamref name="T"/> object to a <typeparamref name="TReturn"/> object.</param>
	/// <returns>List of <see cref="Result{T}"/> objects returned by <paramref name="f"/>.</returns>
	public static IEnumerable<Result<TReturn>> FilterBind<T, TReturn>(this IEnumerable<Result<T>> @this,
		Func<T, bool> fTest,
		Func<T, Result<TReturn>> f
	)
	{
		foreach (var item in @this)
		{
			yield return item.BindIf(fTest, f);
		}
	}

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Result{T}}, Func{T, bool}, Func{T, Result{TReturn}})"/>
	public static Task<List<Result<TReturn>>> FilterBindAsync<T, TReturn>(this IEnumerable<Result<T>> @this,
		Func<T, bool> fTest,
		Func<T, Task<Result<TReturn>>> f
	) =>
		FilterBindAsync(Task.FromResult(@this), fTest, f);

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Result{T}}, Func{T, bool}, Func{T, Result{TReturn}})"/>
	public static Task<List<Result<TReturn>>> FilterBindAsync<T, TReturn>(this Task<IEnumerable<Result<T>>> @this,
		Func<T, bool> fTest,
		Func<T, Result<TReturn>> f
	) =>
		FilterBindAsync(@this, fTest, async x => f(x));

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Result{T}}, Func{T, bool}, Func{T, Result{TReturn}})"/>
	public static async Task<List<Result<TReturn>>> FilterBindAsync<T, TReturn>(this Task<IEnumerable<Result<T>>> @this,
		Func<T, bool> fTest,
		Func<T, Task<Result<TReturn>>> f
	)
	{
		var items = new List<Result<TReturn>>();

		foreach (var item in await @this)
		{
			items.Add(await item.BindIfAsync(fTest, f));
		}

		return items;
	}

	#endregion
}
