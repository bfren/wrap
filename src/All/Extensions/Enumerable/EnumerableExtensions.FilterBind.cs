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
	/// Run <paramref name="bind"/> on each element of <paramref name="this"/> that matches <paramref name="predicate"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">List of Maybe objects.</param>
	/// <param name="predicate">Function to detemine whether or not the value of <paramref name="this"/> should be passed to <paramref name="bind"/>.</param>
	/// <param name="bind">Function to convert a <typeparamref name="T"/> object to a <typeparamref name="TReturn"/> object.</param>
	/// <returns>List of <see cref="Maybe{T}"/> objects returned by <paramref name="bind"/>.</returns>
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
	public static Task<IEnumerable<Maybe<TReturn>>> FilterBindAsync<T, TReturn>(this IEnumerable<Maybe<T>> @this,
		Func<T, bool> predicate,
		Func<T, Task<Maybe<TReturn>>> bind
	) =>
		FilterBindAsync(Task.FromResult(@this), x => Task.FromResult(predicate(x)), bind);

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, Maybe{TReturn}})"/>
	public static Task<IEnumerable<Maybe<TReturn>>> FilterBindAsync<T, TReturn>(this IEnumerable<Maybe<T>> @this,
		Func<T, Task<bool>> predicate,
		Func<T, Maybe<TReturn>> bind
	) =>
		FilterBindAsync(Task.FromResult(@this), predicate, x => bind(x).AsTask());

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, Maybe{TReturn}})"/>
	public static Task<IEnumerable<Maybe<TReturn>>> FilterBindAsync<T, TReturn>(this IEnumerable<Maybe<T>> @this,
		Func<T, Task<bool>> predicate,
		Func<T, Task<Maybe<TReturn>>> bind
	) =>
		FilterBindAsync(Task.FromResult(@this), predicate, bind);

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, Maybe{TReturn}})"/>
	public static Task<IEnumerable<Maybe<TReturn>>> FilterBindAsync<T, TReturn>(this Task<IEnumerable<Maybe<T>>> @this,
		Func<T, bool> predicate,
		Func<T, Maybe<TReturn>> bind
	) =>
		FilterBindAsync(@this, x => Task.FromResult(predicate(x)), x => bind(x).AsTask());

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, Maybe{TReturn}})"/>
	public static Task<IEnumerable<Maybe<TReturn>>> FilterBindAsync<T, TReturn>(this Task<IEnumerable<Maybe<T>>> @this,
		Func<T, bool> predicate,
		Func<T, Task<Maybe<TReturn>>> bind
	) =>
		FilterBindAsync(@this, x => Task.FromResult(predicate(x)), bind);

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, Maybe{TReturn}})"/>
	public static Task<IEnumerable<Maybe<TReturn>>> FilterBindAsync<T, TReturn>(this Task<IEnumerable<Maybe<T>>> @this,
		Func<T, Task<bool>> predicate,
		Func<T, Maybe<TReturn>> bind
	) =>
		FilterBindAsync(@this, predicate, x => Task.FromResult(bind(x)));

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, Maybe{TReturn}})"/>
	public static async Task<IEnumerable<Maybe<TReturn>>> FilterBindAsync<T, TReturn>(this Task<IEnumerable<Maybe<T>>> @this,
		Func<T, Task<bool>> predicate,
		Func<T, Task<Maybe<TReturn>>> bind
	)
	{
		var items = new List<Maybe<TReturn>>();

		foreach (var item in await @this)
		{
			foreach (var some in item)
			{
				if (some is T value && await predicate(value))
				{
					items.Add(await bind(value));
				}
			}
		}

		return items;
	}

	#endregion

	#region Result

	/// <summary>
	/// Run <paramref name="bind"/> on each element of <paramref name="this"/> that matches <paramref name="predicate"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">List of Result objects.</param>
	/// <param name="predicate">Function to detemine whether or not the value of <paramref name="this"/> should be passed to <paramref name="bind"/>.</param>
	/// <param name="bind">Function to convert a <typeparamref name="T"/> object to a <typeparamref name="TReturn"/> object.</param>
	/// <returns>List of <see cref="Result{T}"/> objects returned by <paramref name="bind"/>.</returns>
	public static IEnumerable<Result<TReturn>> FilterBind<T, TReturn>(this IEnumerable<Result<T>> @this,
		Func<T, bool> predicate,
		Func<T, Result<TReturn>> bind
	)
	{
		foreach (var item in @this)
		{
			yield return item.BindIf(x => predicate(x), bind);
		}
	}

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Result{T}}, Func{T, bool}, Func{T, Result{TReturn}})"/>
	public static Task<IEnumerable<Result<TReturn>>> FilterBindAsync<T, TReturn>(this IEnumerable<Result<T>> @this,
		Func<T, bool> predicate,
		Func<T, Task<Result<TReturn>>> bind
	) =>
		FilterBindAsync(Task.FromResult(@this), x => Task.FromResult(predicate(x)), bind);

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Result{T}}, Func{T, bool}, Func{T, Result{TReturn}})"/>
	public static Task<IEnumerable<Result<TReturn>>> FilterBindAsync<T, TReturn>(this IEnumerable<Result<T>> @this,
		Func<T, Task<bool>> predicate,
		Func<T, Result<TReturn>> bind
	) =>
		FilterBindAsync(Task.FromResult(@this), predicate, x => Task.FromResult(bind(x)));

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Result{T}}, Func{T, bool}, Func{T, Result{TReturn}})"/>
	public static Task<IEnumerable<Result<TReturn>>> FilterBindAsync<T, TReturn>(this IEnumerable<Result<T>> @this,
		Func<T, Task<bool>> predicate,
		Func<T, Task<Result<TReturn>>> bind
	) =>
		FilterBindAsync(Task.FromResult(@this), predicate, bind);

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Result{T}}, Func{T, bool}, Func{T, Result{TReturn}})"/>
	public static Task<IEnumerable<Result<TReturn>>> FilterBindAsync<T, TReturn>(this Task<IEnumerable<Result<T>>> @this,
		Func<T, bool> predicate,
		Func<T, Result<TReturn>> bind
	) =>
		FilterBindAsync(@this, x => Task.FromResult(predicate(x)), x => bind(x).AsTask());

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Result{T}}, Func{T, bool}, Func{T, Result{TReturn}})"/>
	public static Task<IEnumerable<Result<TReturn>>> FilterBindAsync<T, TReturn>(this Task<IEnumerable<Result<T>>> @this,
		Func<T, bool> predicate,
		Func<T, Task<Result<TReturn>>> bind
	) =>
		FilterBindAsync(@this, x => Task.FromResult(predicate(x)), bind);

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Result{T}}, Func{T, bool}, Func{T, Result{TReturn}})"/>
	public static Task<IEnumerable<Result<TReturn>>> FilterBindAsync<T, TReturn>(this Task<IEnumerable<Result<T>>> @this,
		Func<T, Task<bool>> predicate,
		Func<T, Result<TReturn>> bind
	) =>
		FilterBindAsync(@this, predicate, x => bind(x).AsTask());

	/// <inheritdoc cref="FilterBind{T, TReturn}(IEnumerable{Result{T}}, Func{T, bool}, Func{T, Result{TReturn}})"/>
	public static async Task<IEnumerable<Result<TReturn>>> FilterBindAsync<T, TReturn>(this Task<IEnumerable<Result<T>>> @this,
		Func<T, Task<bool>> predicate,
		Func<T, Task<Result<TReturn>>> bind
	)
	{
		var items = new List<Result<TReturn>>();

		foreach (var item in await @this)
		{
			items.Add(await item.BindIfAsync(predicate, bind));
		}

		return items;
	}

	#endregion
}
