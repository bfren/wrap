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
	/// Run <paramref name="f"/> on each element of <paramref name="this"/> that matches <paramref name="predicate"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">List of Maybe objects.</param>
	/// <param name="predicate">Function to detemine whether or not the value of <paramref name="this"/> should be passed to <paramref name="f"/>.</param>
	/// <param name="f">Function to convert a <typeparamref name="T"/> object to a <typeparamref name="TReturn"/> object.</param>
	/// <returns>List of <see cref="Maybe{T}"/> objects returned by <paramref name="f"/>.</returns>
	public static IEnumerable<Maybe<TReturn>> FilterMap<T, TReturn>(this IEnumerable<T> @this,
		Func<T, bool> predicate,
		Func<T, Maybe<TReturn>> f
	)
	{
		foreach (var item in @this)
		{
			if (item is T value && predicate(value))
			{
				yield return f(value);
			}
		}
	}

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{T}, Func{T, bool}, Func{T, Maybe{TReturn}})"/>
	public static Task<List<Maybe<TReturn>>> FilterMapAsync<T, TReturn>(this IEnumerable<T> @this,
		Func<T, bool> predicate,
		Func<T, Task<Maybe<TReturn>>> f
	) =>
		FilterMapAsync(Task.FromResult(@this), x => Task.FromResult(predicate(x)), f);

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{T}, Func{T, bool}, Func{T, Maybe{TReturn}})"/>
	public static Task<List<Maybe<TReturn>>> FilterMapAsync<T, TReturn>(this IEnumerable<T> @this,
		Func<T, Task<bool>> predicate,
		Func<T, Maybe<TReturn>> f
	) =>
		FilterMapAsync(Task.FromResult(@this), predicate, x => Task.FromResult(f(x)));

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{T}, Func{T, bool}, Func{T, Maybe{TReturn}})"/>
	public static Task<List<Maybe<TReturn>>> FilterMapAsync<T, TReturn>(this IEnumerable<T> @this,
		Func<T, Task<bool>> predicate,
		Func<T, Task<Maybe<TReturn>>> f
	) =>
		FilterMapAsync(Task.FromResult(@this), predicate, f);

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{T}, Func{T, bool}, Func{T, Maybe{TReturn}})"/>
	public static Task<List<Maybe<TReturn>>> FilterMapAsync<T, TReturn>(this Task<IEnumerable<T>> @this,
		Func<T, bool> predicate,
		Func<T, Maybe<TReturn>> f
	) =>
		FilterMapAsync(@this, x => Task.FromResult(predicate(x)), x => Task.FromResult(f(x)));

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{T}, Func{T, bool}, Func{T, Maybe{TReturn}})"/>
	public static Task<List<Maybe<TReturn>>> FilterMapAsync<T, TReturn>(this Task<IEnumerable<T>> @this,
		Func<T, bool> predicate,
		Func<T, Task<Maybe<TReturn>>> f
	) =>
		FilterMapAsync(@this, x => Task.FromResult(predicate(x)), f);

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{T}, Func{T, bool}, Func{T, Maybe{TReturn}})"/>
	public static Task<List<Maybe<TReturn>>> FilterMapAsync<T, TReturn>(this Task<IEnumerable<T>> @this,
		Func<T, Task<bool>> predicate,
		Func<T, Maybe<TReturn>> f
	) =>
		FilterMapAsync(@this, predicate, x => Task.FromResult(f(x)));

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{T}, Func{T, bool}, Func{T, Maybe{TReturn}})"/>
	public static async Task<List<Maybe<TReturn>>> FilterMapAsync<T, TReturn>(this Task<IEnumerable<T>> @this,
		Func<T, Task<bool>> predicate,
		Func<T, Task<Maybe<TReturn>>> f
	)
	{
		var list = new List<Maybe<TReturn>>();

		foreach (var item in await @this)
		{
			if (item is T value && await predicate(value))
			{
				list.Add(await f(value));
			}
		}

		return list;
	}

	#endregion

	#region Result

	/// <summary>
	/// Run <paramref name="f"/> on each element of <paramref name="this"/> that matches <paramref name="predicate"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">List of Result objects.</param>
	/// <param name="predicate">Function to detemine whether or not the value of <paramref name="this"/> should be passed to <paramref name="f"/>.</param>
	/// <param name="f">Function to convert a <typeparamref name="T"/> object to a <typeparamref name="TReturn"/> object.</param>
	/// <returns>List of <see cref="Result{T}"/> objects returned by <paramref name="f"/>.</returns>
	public static IEnumerable<Result<TReturn>> FilterMap<T, TReturn>(this IEnumerable<T> @this,
		Func<T, bool> predicate,
		Func<T, Result<TReturn>> f
	)
	{
		foreach (var item in @this)
		{
			if (item is T value && predicate(value))
			{
				yield return f(value);
			}
		}
	}

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{T}, Func{T, bool}, Func{T, Result{TReturn}})"/>
	public static Task<List<Result<TReturn>>> FilterMapAsync<T, TReturn>(this IEnumerable<T> @this,
		Func<T, bool> predicate,
		Func<T, Task<Result<TReturn>>> f
	) =>
		FilterMapAsync(Task.FromResult(@this), x => Task.FromResult(predicate(x)), f);

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{T}, Func{T, bool}, Func{T, Result{TReturn}})"/>
	public static Task<List<Result<TReturn>>> FilterMapAsync<T, TReturn>(this IEnumerable<T> @this,
		Func<T, Task<bool>> predicate,
		Func<T, Result<TReturn>> f
	) =>
		FilterMapAsync(Task.FromResult(@this), predicate, x => Task.FromResult(f(x)));

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{T}, Func{T, bool}, Func{T, Result{TReturn}})"/>
	public static Task<List<Result<TReturn>>> FilterMapAsync<T, TReturn>(this IEnumerable<T> @this,
		Func<T, Task<bool>> predicate,
		Func<T, Task<Result<TReturn>>> f
	) =>
		FilterMapAsync(Task.FromResult(@this), predicate, f);

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{T}, Func{T, bool}, Func{T, Result{TReturn}})"/>
	public static Task<List<Result<TReturn>>> FilterMapAsync<T, TReturn>(this Task<IEnumerable<T>> @this,
		Func<T, bool> predicate,
		Func<T, Result<TReturn>> f
	) =>
		FilterMapAsync(@this, x => Task.FromResult(predicate(x)), x => Task.FromResult(f(x)));

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{T}, Func{T, bool}, Func{T, Result{TReturn}})"/>
	public static Task<List<Result<TReturn>>> FilterMapAsync<T, TReturn>(this Task<IEnumerable<T>> @this,
		Func<T, bool> predicate,
		Func<T, Task<Result<TReturn>>> f
	) =>
		FilterMapAsync(@this, x => Task.FromResult(predicate(x)), f);

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{T}, Func{T, bool}, Func{T, Result{TReturn}})"/>
	public static Task<List<Result<TReturn>>> FilterMapAsync<T, TReturn>(this Task<IEnumerable<T>> @this,
		Func<T, Task<bool>> predicate,
		Func<T, Result<TReturn>> f
	) =>
		FilterMapAsync(@this, predicate, x => Task.FromResult(f(x)));

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{T}, Func{T, bool}, Func{T, Result{TReturn}})"/>
	public static async Task<List<Result<TReturn>>> FilterMapAsync<T, TReturn>(this Task<IEnumerable<T>> @this,
		Func<T, Task<bool>> predicate,
		Func<T, Task<Result<TReturn>>> f
	)
	{
		var list = new List<Result<TReturn>>();

		foreach (var item in await @this)
		{
			if (item is T value && await predicate(value))
			{
				list.Add(await f(value));
			}
		}

		return list;
	}

	#endregion
}
