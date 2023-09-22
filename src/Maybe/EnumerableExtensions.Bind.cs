// Monads: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monads;

public static partial class EnumerableExtensions
{
	/// <summary>
	/// Run <paramref name="bind"/> on each value in <paramref name="this"/> that is <see cref="Some{T}"/>.
	/// </summary>
	/// <typeparam name="T">Maybe value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">List of Maybe objects.</param>
	/// <param name="bind">Function to convert a <typeparamref name="T"/> object to a <typeparamref name="TReturn"/> object.</param>
	/// <returns>List of <see cref="Maybe{T}"/> objects returned by <paramref name="bind"/>.</returns>
	public static IEnumerable<Maybe<TReturn>> Bind<T, TReturn>(this IEnumerable<Maybe<T>> @this, Func<T, Maybe<TReturn>> bind)
	{
		foreach (var item in @this)
		{
			foreach (var value in item)
			{
				if (value is not null)
				{
					yield return bind(value);
				}
			}
		}
	}

	/// <inheritdoc cref="Bind{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, Maybe{TReturn}})"/>
	public static async IAsyncEnumerable<Maybe<TReturn>> BindAsync<T, TReturn>(this IEnumerable<Maybe<T>> @this, Func<T, Task<Maybe<TReturn>>> bind)
	{
		foreach (var item in @this)
		{
			foreach (var value in item)
			{
				if (value is not null)
				{
					yield return await bind(value);
				}
			}
		}
	}

	/// <inheritdoc cref="Bind{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, Maybe{TReturn}})"/>
	public static IAsyncEnumerable<Maybe<TReturn>> BindAsync<T, TReturn>(this IAsyncEnumerable<Maybe<T>> @this, Func<T, Maybe<TReturn>> bind) =>
		BindAsync(@this, x => Task.FromResult(bind(x)));

	/// <inheritdoc cref="Bind{T, TReturn}(IEnumerable{Maybe{T}}, Func{T, Maybe{TReturn}})"/>
	public static async IAsyncEnumerable<Maybe<TReturn>> BindAsync<T, TReturn>(this IAsyncEnumerable<Maybe<T>> @this, Func<T, Task<Maybe<TReturn>>> bind)
	{
		await foreach (var item in @this)
		{
			foreach (var value in item)
			{
				if (value is not null)
				{
					yield return await bind(value);
				}
			}
		}
	}
}
