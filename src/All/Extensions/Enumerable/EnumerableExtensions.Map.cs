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
	/// Convert the value of each <see cref="Some{T}"/> item in <paramref name="this"/> with <paramref name="f"/>.
	/// </summary>
	/// <remarks>
	/// <see cref="None"/> items are kept in place. The result is lazy; the async overloads build the whole list.
	/// </remarks>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">List of Maybe objects.</param>
	/// <param name="f">Function that converts each value.</param>
	/// <returns>
	/// One item for each item in <paramref name="this"/> - see <see cref="MaybeExtensions.Map{T, TReturn}(Maybe{T}, Func{T, TReturn})"/>.
	/// </returns>
	public static IEnumerable<Maybe<TReturn>> Map<T, TReturn>(this IEnumerable<Maybe<T>> @this, Func<T, TReturn> f)
	{
		foreach (var item in @this)
		{
			yield return item.Map(f);
		}
	}

	/// <inheritdoc cref="Map{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, TReturn})"/>
	public static async Task<IEnumerable<Maybe<TReturn>>> MapAsync<T, TReturn>(this IEnumerable<Maybe<T>> @this, Func<T, Task<TReturn>> f)
	{
		var list = new List<Maybe<TReturn>>();

		foreach (var item in @this)
		{
			list.Add(await item.MapAsync(f));
		}

		return list;
	}

	/// <inheritdoc cref="Map{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, TReturn})"/>
	public static Task<IEnumerable<Maybe<TReturn>>> MapAsync<T, TReturn>(this Task<IEnumerable<Maybe<T>>> @this, Func<T, TReturn> f) =>
		@this.Then(x => x.Map(f));

	/// <inheritdoc cref="Map{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, TReturn})"/>
	public static Task<IEnumerable<Maybe<TReturn>>> MapAsync<T, TReturn>(this Task<IEnumerable<Maybe<T>>> @this, Func<T, Task<TReturn>> f) =>
		@this.Then(x => x.MapAsync(f));

	#endregion

	#region Result

	/// <summary>
	/// Convert the value of each <see cref="Ok{T}"/> item in <paramref name="this"/> with <paramref name="f"/>.
	/// </summary>
	/// <remarks>
	/// <see cref="Failure"/> items are kept in place. The result is lazy; the async overloads build the whole list.
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">List of Result objects.</param>
	/// <param name="f">Function that converts each value.</param>
	/// <returns>
	/// One item for each item in <paramref name="this"/> - see <see cref="ResultExtensions.Map{T, TReturn}(Result{T}, Func{T, TReturn})"/>.
	/// </returns>
	public static IEnumerable<Result<TReturn>> Map<T, TReturn>(this IEnumerable<Result<T>> @this, Func<T, TReturn> f)
	{
		foreach (var item in @this)
		{
			yield return item.Map(f);
		}
	}

	/// <inheritdoc cref="Map{T, TReturn}(IEnumerable{Result{T}}, Func{T, TReturn})"/>
	public static async Task<IEnumerable<Result<TReturn>>> MapAsync<T, TReturn>(this IEnumerable<Result<T>> @this, Func<T, Task<TReturn>> f)
	{
		var list = new List<Result<TReturn>>();

		foreach (var item in @this)
		{
			list.Add(await item.MapAsync(f));
		}

		return list;
	}

	/// <inheritdoc cref="Map{T, TReturn}(IEnumerable{Result{T}}, Func{T, TReturn})"/>
	public static Task<IEnumerable<Result<TReturn>>> MapAsync<T, TReturn>(this Task<IEnumerable<Result<T>>> @this, Func<T, TReturn> f) =>
		@this.Then(x => x.Map(f));

	/// <inheritdoc cref="Map{T, TReturn}(IEnumerable{Result{T}}, Func{T, TReturn})"/>
	public static Task<IEnumerable<Result<TReturn>>> MapAsync<T, TReturn>(this Task<IEnumerable<Result<T>>> @this, Func<T, Task<TReturn>> f) =>
		@this.Then(x => x.MapAsync(f));

	#endregion
}
