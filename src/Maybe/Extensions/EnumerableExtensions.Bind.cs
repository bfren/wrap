// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class EnumerableExtensions
{
	/// <summary>
	/// Run <paramref name="f"/> on each value in <paramref name="this"/> that is <see cref="Some{T}"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">List of Maybe objects.</param>
	/// <param name="f">Function to convert a <typeparamref name="T"/> object to a <typeparamref name="TReturn"/> object.</param>
	/// <returns>List of <see cref="Maybe{T}"/> objects returned by <paramref name="f"/>.</returns>
	public static IEnumerable<Maybe<TReturn>> Bind<T, TReturn>(this IEnumerable<Maybe<T>> @this, Func<T, Maybe<TReturn>> f)
	{
		foreach (var item in @this)
		{
			foreach (var value in item)
			{
				if (value is not null)
				{
					yield return f(value);
				}
			}
		}
	}

	/// <inheritdoc cref="Bind{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, Maybe{TReturn}})"/>
	public static async IAsyncEnumerable<Maybe<TReturn>> BindAsync<T, TReturn>(this IEnumerable<Maybe<T>> @this, Func<T, Task<Maybe<TReturn>>> f)
	{
		foreach (var item in @this)
		{
			foreach (var value in item)
			{
				if (value is not null)
				{
					yield return await f(value);
				}
			}
		}
	}

	/// <inheritdoc cref="Bind{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, Maybe{TReturn}})"/>
	public static IAsyncEnumerable<Maybe<TReturn>> BindAsync<T, TReturn>(this IAsyncEnumerable<Maybe<T>> @this, Func<T, Maybe<TReturn>> f) =>
		BindAsync(@this, x => Task.FromResult(f(x)));

	/// <inheritdoc cref="Bind{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, Maybe{TReturn}})"/>
	public static async IAsyncEnumerable<Maybe<TReturn>> BindAsync<T, TReturn>(this IAsyncEnumerable<Maybe<T>> @this, Func<T, Task<Maybe<TReturn>>> f)
	{
		await foreach (var item in @this)
		{
			foreach (var value in item)
			{
				if (value is not null)
				{
					yield return await f(value);
				}
			}
		}
	}
}
