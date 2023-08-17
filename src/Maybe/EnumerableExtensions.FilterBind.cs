// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monadic;

public static partial class EnumerableExtensions
{
	/// <summary>
	/// Run <paramref name="bind"/> on each element of <paramref name="this"/> that matches <paramref name="predicate"/>.
	/// </summary>
	/// <typeparam name="T">Maybe value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">List of Maybe objects.</param>
	/// <param name="predicate">Function to detemine whether or not the value of <paramref name="this"/> should be passed to <paramref name="bind"/>.</param>
	/// <param name="bind">Function to convert a <typeparamref name="T"/> object to a <typeparamref name="TReturn"/> object.</param>
	/// <returns>List of <see cref="Maybe{TReturn}"/> objects returned by <paramref name="bind"/>.</returns>
	public static IEnumerable<Maybe<TReturn>> FilterBind<T, TReturn>(this IEnumerable<Maybe<T>> @this,
		Func<T, bool> predicate,
		Func<T, Maybe<TReturn>> bind
	)
	{
		foreach (var item in @this)
		{
			foreach (var some in item)
			{
				if (some is T value && predicate(value))
				{
					yield return bind(value);
				}
			}
		}
	}

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, Maybe{TReturn}})"/>
	public static IAsyncEnumerable<Maybe<TReturn>> FilterBindAsync<T, TReturn>(this IEnumerable<Maybe<T>> @this,
		Func<T, bool> predicate,
		Func<T, Task<Maybe<TReturn>>> bind
	) =>
		FilterBindAsync(@this, x => Task.FromResult(predicate(x)), bind);

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, Maybe{TReturn}})"/>
	public static IAsyncEnumerable<Maybe<TReturn>> FilterBindAsync<T, TReturn>(this IEnumerable<Maybe<T>> @this,
		Func<T, Task<bool>> predicate,
		Func<T, Maybe<TReturn>> bind
	) =>
		FilterBindAsync(@this, predicate, x => Task.FromResult(bind(x)));

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, Maybe{TReturn}})"/>
	public static async IAsyncEnumerable<Maybe<TReturn>> FilterBindAsync<T, TReturn>(this IEnumerable<Maybe<T>> @this,
		Func<T, Task<bool>> predicate,
		Func<T, Task<Maybe<TReturn>>> bind
	)
	{
		foreach (var item in @this)
		{
			foreach (var some in item)
			{
				if (some is T value && await predicate(value))
				{
					yield return await bind(value);
				}
			}
		}
	}

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, Maybe{TReturn}})"/>
	public static IAsyncEnumerable<Maybe<TReturn>> FilterBindAsync<T, TReturn>(this IAsyncEnumerable<Maybe<T>> @this,
		Func<T, bool> predicate,
		Func<T, Task<Maybe<TReturn>>> bind
	) =>
		FilterBindAsync(@this, x => Task.FromResult(predicate(x)), bind);

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, Maybe{TReturn}})"/>
	public static IAsyncEnumerable<Maybe<TReturn>> FilterBindAsync<T, TReturn>(this IAsyncEnumerable<Maybe<T>> @this,
		Func<T, Task<bool>> predicate,
		Func<T, Maybe<TReturn>> bind
	) =>
		FilterBindAsync(@this, predicate, x => Task.FromResult(bind(x)));

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, Maybe{TReturn}})"/>
	public static async IAsyncEnumerable<Maybe<TReturn>> FilterBindAsync<T, TReturn>(this IAsyncEnumerable<Maybe<T>> @this,
		Func<T, Task<bool>> predicate,
		Func<T, Task<Maybe<TReturn>>> bind
	)
	{
		await foreach (var item in @this)
		{
			foreach (var some in item)
			{
				if (some is T value && await predicate(value))
				{
					yield return await bind(value);
				}
			}
		}
	}
}
