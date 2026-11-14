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
	/// Run <paramref name="f"/> on each element of <paramref name="this"/> that matches <paramref name="fTest"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">List of Maybe objects.</param>
	/// <param name="fTest">Function to detemine whether or not the value of <paramref name="this"/> should be passed to <paramref name="f"/>.</param>
	/// <param name="f">Function to convert a <typeparamref name="T"/> object to a <typeparamref name="TReturn"/> object.</param>
	/// <returns>List of <see cref="Maybe{T}"/> objects returned by <paramref name="f"/>.</returns>
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
	/// Run <paramref name="f"/> on each element of <paramref name="this"/> that matches <paramref name="fTest"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">List of Result objects.</param>
	/// <param name="fTest">Function to detemine whether or not the value of <paramref name="this"/> should be passed to <paramref name="f"/>.</param>
	/// <param name="f">Function to convert a <typeparamref name="T"/> object to a <typeparamref name="TReturn"/> object.</param>
	/// <returns>List of <see cref="Result{T}"/> objects returned by <paramref name="f"/>.</returns>
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
