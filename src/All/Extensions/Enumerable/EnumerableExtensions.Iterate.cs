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
	/// Loop through list <paramref name="this"/> and apply function <paramref name="f"/> to each element
	/// that is a <see cref="Some{T}"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">List of <see cref="Maybe{T}"/> objects.</param>
	/// <param name="f">Function to apply to each <see cref="Some{T}"/> element of <paramref name="this"/>.</param>
	public static void Iterate<T>(this IEnumerable<Maybe<T>> @this, Action<T> f)
	{
		foreach (var item in @this)
		{
			foreach (var value in item)
			{
				f(value);
			}
		}
	}

	/// <inheritdoc cref="Iterate{T}(IEnumerable{Maybe{T}}, Action{T})"/>
	public static Task IterateAsync<T>(this IEnumerable<Maybe<T>> @this, Func<T, Task> f) =>
		IterateAsync(Task.FromResult(@this), f);

	/// <inheritdoc cref="Iterate{T}(IEnumerable{Maybe{T}}, Action{T})"/>
	public static Task IterateAsync<T>(this Task<IEnumerable<Maybe<T>>> @this, Action<T> f) =>
		IterateAsync(@this, async x => f(x));

	/// <inheritdoc cref="Iterate{T}(IEnumerable{Maybe{T}}, Action{T})"/>
	public static async Task IterateAsync<T>(this Task<IEnumerable<Maybe<T>>> @this, Func<T, Task> f)
	{
		foreach (var item in await @this)
		{
			foreach (var value in item)
			{
				await f(value);
			}
		}
	}

	#endregion

	#region Result

	/// <summary>
	/// Loop through list <paramref name="this"/> and apply function <paramref name="f"/> to each element
	/// that is a <see cref="Ok{T}"/>.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">List of <see cref="Result{T}"/> objects.</param>
	/// <param name="f">Function to apply to each <see cref="Ok{T}"/> element of <paramref name="this"/>.</param>
	public static void Iterate<T>(this IEnumerable<Result<T>> @this, Action<T> f)
	{
		foreach (var item in @this)
		{
			foreach (var value in item.Unsafe())
			{
				f(value);
			}
		}
	}

	/// <inheritdoc cref="Iterate{T}(IEnumerable{Result{T}}, Action{T})"/>
	public static Task IterateAsync<T>(this IEnumerable<Result<T>> @this, Func<T, Task> f) =>
		IterateAsync(Task.FromResult(@this), f);

	/// <inheritdoc cref="Iterate{T}(IEnumerable{Result{T}}, Action{T})"/>
	public static Task IterateAsync<T>(this Task<IEnumerable<Result<T>>> @this, Action<T> f) =>
		IterateAsync(@this, async x => f(x));

	/// <inheritdoc cref="Iterate{T}(IEnumerable{Result{T}}, Action{T})"/>
	public static async Task IterateAsync<T>(this Task<IEnumerable<Result<T>>> @this, Func<T, Task> f)
	{
		foreach (var item in await @this)
		{
			foreach (var value in item.Unsafe())
			{
				await f(value);
			}
		}
	}

	#endregion
}
