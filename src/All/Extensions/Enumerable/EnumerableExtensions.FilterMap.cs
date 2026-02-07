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
	public static IEnumerable<Maybe<TReturn>> FilterMap<T, TReturn>(this IEnumerable<Maybe<T>> @this,
		Func<T, bool> fTest,
		Func<T, TReturn> f
	)
	{
		foreach (var item in @this.Filter())
		{
			foreach (var value in item)
			{
				if (value is not null && fTest(value))
				{
					yield return f(value);
				}
			}
		}
	}

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, TReturn})"/>
	public static Task<List<Maybe<TReturn>>> FilterMapAsync<T, TReturn>(this IEnumerable<Maybe<T>> @this,
		Func<T, bool> fTest,
		Func<T, Task<TReturn>> f
	) =>
		FilterMapAsync(Task.FromResult(@this), async x => fTest(x), f);

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, TReturn})"/>
	public static Task<List<Maybe<TReturn>>> FilterMapAsync<T, TReturn>(this IEnumerable<Maybe<T>> @this,
		Func<T, Task<bool>> fTest,
		Func<T, TReturn> f
	) =>
		FilterMapAsync(Task.FromResult(@this), fTest, async x => f(x));

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, TReturn})"/>
	public static Task<List<Maybe<TReturn>>> FilterMapAsync<T, TReturn>(this IEnumerable<Maybe<T>> @this,
		Func<T, Task<bool>> fTest,
		Func<T, Task<TReturn>> f
	) =>
		FilterMapAsync(Task.FromResult(@this), fTest, f);

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, TReturn})"/>
	public static Task<List<Maybe<TReturn>>> FilterMapAsync<T, TReturn>(this Task<IEnumerable<Maybe<T>>> @this,
		Func<T, bool> fTest,
		Func<T, TReturn> f
	) =>
		FilterMapAsync(@this, async x => fTest(x), async x => f(x));

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, TReturn})"/>
	public static Task<List<Maybe<TReturn>>> FilterMapAsync<T, TReturn>(this Task<IEnumerable<Maybe<T>>> @this,
		Func<T, bool> fTest,
		Func<T, Task<TReturn>> f
	) =>
		FilterMapAsync(@this, async x => fTest(x), f);

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, TReturn})"/>
	public static Task<List<Maybe<TReturn>>> FilterMapAsync<T, TReturn>(this Task<IEnumerable<Maybe<T>>> @this,
		Func<T, Task<bool>> fTest,
		Func<T, TReturn> f
	) =>
		FilterMapAsync(@this, fTest, async x => f(x));

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, bool}, Func{T, TReturn})"/>
	public static async Task<List<Maybe<TReturn>>> FilterMapAsync<T, TReturn>(this Task<IEnumerable<Maybe<T>>> @this,
		Func<T, Task<bool>> fTest,
		Func<T, Task<TReturn>> f
	)
	{
		var list = new List<Maybe<TReturn>>();

		foreach (var item in await @this.FilterAsync())
		{
			foreach (var value in item)
			{
				if (value is not null && await fTest(value))
				{
					list.Add(await f(value));
				}
			}
		}

		return list;
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
	public static IEnumerable<Result<TReturn>> FilterMap<T, TReturn>(this IEnumerable<Result<T>> @this,
		Func<T, bool> fTest,
		Func<T, TReturn> f
	)
	{
		foreach (var item in @this)
		{
			yield return item.Bind(x => fTest(x) switch
			{
				true =>
					R.Try(() => f(x)),

				false =>
					R.Fail(C.TestFalseMessage).Ctx(nameof(EnumerableExtensions), nameof(FilterMap))
			});
		}
	}

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{Result{T}}, Func{T, bool}, Func{T, TReturn})"/>
	public static Task<List<Result<TReturn>>> FilterMapAsync<T, TReturn>(this IEnumerable<Result<T>> @this,
		Func<T, bool> fTest,
		Func<T, Task<TReturn>> f
	) =>
		FilterMapAsync(Task.FromResult(@this), async x => fTest(x), f);

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{Result{T}}, Func{T, bool}, Func{T, TReturn})"/>
	public static Task<List<Result<TReturn>>> FilterMapAsync<T, TReturn>(this IEnumerable<Result<T>> @this,
		Func<T, Task<bool>> fTest,
		Func<T, TReturn> f
	) =>
		FilterMapAsync(Task.FromResult(@this), fTest, async x => f(x));

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{Result{T}}, Func{T, bool}, Func{T, TReturn})"/>
	public static Task<List<Result<TReturn>>> FilterMapAsync<T, TReturn>(this IEnumerable<Result<T>> @this,
		Func<T, Task<bool>> fTest,
		Func<T, Task<TReturn>> f
	) =>
		FilterMapAsync(Task.FromResult(@this), fTest, f);

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{Result{T}}, Func{T, bool}, Func{T, TReturn})"/>
	public static Task<List<Result<TReturn>>> FilterMapAsync<T, TReturn>(this Task<IEnumerable<Result<T>>> @this,
		Func<T, bool> fTest,
		Func<T, TReturn> f
	) =>
		FilterMapAsync(@this, async x => fTest(x), async x => f(x));

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{Result{T}}, Func{T, bool}, Func{T, TReturn})"/>
	public static Task<List<Result<TReturn>>> FilterMapAsync<T, TReturn>(this Task<IEnumerable<Result<T>>> @this,
		Func<T, bool> fTest,
		Func<T, Task<TReturn>> f
	) =>
		FilterMapAsync(@this, async x => fTest(x), f);

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{Result{T}}, Func{T, bool}, Func{T, TReturn})"/>
	public static Task<List<Result<TReturn>>> FilterMapAsync<T, TReturn>(this Task<IEnumerable<Result<T>>> @this,
		Func<T, Task<bool>> fTest,
		Func<T, TReturn> f
	) =>
		FilterMapAsync(@this, fTest, async x => f(x));

	/// <inheritdoc cref="FilterMap{T, TReturn}(IEnumerable{Result{T}}, Func{T, bool}, Func{T, TReturn})"/>
	public static async Task<List<Result<TReturn>>> FilterMapAsync<T, TReturn>(this Task<IEnumerable<Result<T>>> @this,
		Func<T, Task<bool>> fTest,
		Func<T, Task<TReturn>> f
	)
	{
		var list = new List<Result<TReturn>>();

		foreach (var item in await @this)
		{
			var value = await item.BindAsync(async x => await fTest(x) switch
			{
				true =>
					await R.TryAsync(() => f(x)),

				false =>
					R.Fail(C.TestFalseMessage).Ctx(nameof(EnumerableExtensions), nameof(FilterMap))
			});

			list.Add(value);
		}

		return list;
	}

	#endregion
}
