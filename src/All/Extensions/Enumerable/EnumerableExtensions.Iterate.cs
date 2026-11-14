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
	/// Run <paramref name="f"/> with the value of each item in <paramref name="this"/> that is <see cref="Some{T}"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">List of Maybe objects.</param>
	/// <param name="f">Action to run with each value; <see cref="None"/> items are skipped.</param>
	public static void Iterate<T>(this IEnumerable<Maybe<T>> @this, Action<T> f)
	{
		foreach (var item in @this)
		{
			if (item is Some<T>(var value))
			{
				f(value);
			}
		}
	}

	/// <inheritdoc cref="Iterate{T}(IEnumerable{Maybe{T}}, Action{T})"/>
	public static async Task IterateAsync<T>(this IEnumerable<Maybe<T>> @this, Func<T, Task> f)
	{
		foreach (var item in @this)
		{
			if (item is Some<T>(var value))
			{
				await f(value);
			}
		}
	}

	/// <inheritdoc cref="Iterate{T}(IEnumerable{Maybe{T}}, Action{T})"/>
	public static Task IterateAsync<T>(this Task<IEnumerable<Maybe<T>>> @this, Action<T> f) =>
		@this.Then(x => x.Iterate(f));

	/// <inheritdoc cref="Iterate{T}(IEnumerable{Maybe{T}}, Action{T})"/>
	public static Task IterateAsync<T>(this Task<IEnumerable<Maybe<T>>> @this, Func<T, Task> f) =>
		@this.Then(x => x.IterateAsync(f));

	#endregion

	#region Result

	/// <summary>
	/// Run <paramref name="f"/> with the value of each item in <paramref name="this"/> that is <see cref="Ok{T}"/>.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">List of Result objects.</param>
	/// <param name="f">Action to run with each value; <see cref="Failure"/> items are skipped.</param>
	public static void Iterate<T>(this IEnumerable<Result<T>> @this, Action<T> f)
	{
		foreach (var item in @this)
		{
			if (item is Ok<T>(var value))
			{
				f(value);
			}
		}
	}

	/// <inheritdoc cref="Iterate{T}(IEnumerable{Result{T}}, Action{T})"/>
	public static async Task IterateAsync<T>(this IEnumerable<Result<T>> @this, Func<T, Task> f)
	{
		foreach (var item in @this)
		{
			if (item is Ok<T>(var value))
			{
				await f(value);
			}
		}
	}

	/// <inheritdoc cref="Iterate{T}(IEnumerable{Result{T}}, Action{T})"/>
	public static Task IterateAsync<T>(this Task<IEnumerable<Result<T>>> @this, Action<T> f) =>
		@this.Then(x => x.Iterate(f));

	/// <inheritdoc cref="Iterate{T}(IEnumerable{Result{T}}, Action{T})"/>
	public static Task IterateAsync<T>(this Task<IEnumerable<Result<T>>> @this, Func<T, Task> f) =>
		@this.Then(x => x.IterateAsync(f));

	#endregion
}
