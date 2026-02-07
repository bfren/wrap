// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class EnumerableExtensions
{
	#region Maybe

	/// <summary>1
	/// Run <paramref name="f"/> on each value in <paramref name="this"/>.
	/// </summary>
	/// <typeparam name="T">Value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">List of values.</param>
	/// <param name="f">Function to convert a <typeparamref name="T"/> object to a <typeparamref name="TReturn"/> object.</param>
	/// <returns>List of <typeparamref name="TReturn"/> objects returned by <paramref name="f"/> and wrapped as <see cref="Maybe{T}"/>.</returns>
	public static IEnumerable<Maybe<TReturn>> Map<T, TReturn>(this IEnumerable<Maybe<T>> @this, Func<T, TReturn> f)
	{
		foreach (var item in @this)
		{
			yield return item.Map(f);
		}
	}

	/// <inheritdoc cref="Map{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, TReturn})"/>
	public static Task<List<Maybe<TReturn>>> MapAsync<T, TReturn>(this IEnumerable<Maybe<T>> @this, Func<T, Task<TReturn>> f) =>
		MapAsync(Task.FromResult(@this), f);

	/// <inheritdoc cref="Map{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, TReturn})"/>
	public static Task<List<Maybe<TReturn>>> MapAsync<T, TReturn>(this Task<IEnumerable<Maybe<T>>> @this, Func<T, TReturn> f) =>
		MapAsync(@this, async x => f(x));

	/// <inheritdoc cref="Map{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, TReturn})"/>
	public static async Task<List<Maybe<TReturn>>> MapAsync<T, TReturn>(this Task<IEnumerable<Maybe<T>>> @this, Func<T, Task<TReturn>> f)
	{
		var list = new List<Maybe<TReturn>>();

		foreach (var item in await @this)
		{
			list.Add(await item.MapAsync(f));
		}

		return list;
	}

	#endregion

	#region Result

	/// <summary>1
	/// Run <paramref name="f"/> on each value in <paramref name="this"/>.
	/// </summary>
	/// <typeparam name="T">Value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">List of values.</param>
	/// <param name="f">Function to convert a <typeparamref name="T"/> object to a <typeparamref name="TReturn"/> object.</param>
	/// <returns>List of <typeparamref name="TReturn"/> objects returned by <paramref name="f"/> and wrapped as <see cref="Result{T}"/>.</returns>
	public static IEnumerable<Result<TReturn>> Map<T, TReturn>(this IEnumerable<Result<T>> @this, Func<T, TReturn> f)
	{
		foreach (var item in @this)
		{
			yield return item.Map(f);
		}
	}

	/// <inheritdoc cref="Map{T, TReturn}(IEnumerable{Result{T}}, Func{T, TReturn})"/>
	public static Task<List<Result<TReturn>>> MapAsync<T, TReturn>(this IEnumerable<Result<T>> @this, Func<T, Task<TReturn>> f) =>
		MapAsync(Task.FromResult(@this), f);

	/// <inheritdoc cref="Map{T, TReturn}(IEnumerable{Result{T}}, Func{T, TReturn})"/>
	public static Task<List<Result<TReturn>>> MapAsync<T, TReturn>(this Task<IEnumerable<Result<T>>> @this, Func<T, TReturn> f) =>
		MapAsync(@this, async x => f(x));

	/// <inheritdoc cref="Map{T, TReturn}(IEnumerable{Result{T}}, Func{T, TReturn})"/>
	public static async Task<List<Result<TReturn>>> MapAsync<T, TReturn>(this Task<IEnumerable<Result<T>>> @this, Func<T, Task<TReturn>> f)
	{
		var list = new List<Result<TReturn>>();

		foreach (var item in await @this)
		{
			list.Add(await item.MapAsync(f));
		}

		return list;
	}

	#endregion
}
