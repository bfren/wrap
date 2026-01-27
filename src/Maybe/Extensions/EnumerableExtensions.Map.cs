// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class EnumerableExtensions
{
	/// <summary>
	/// Run <paramref name="map"/> on each value in <paramref name="this"/>.
	/// </summary>
	/// <typeparam name="T">Value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">List of values.</param>
	/// <param name="map">Function to convert a <typeparamref name="T"/> object to a <typeparamref name="TReturn"/> object.</param>
	/// <returns>List of <typeparamref name="TReturn"/> objects returned by <paramref name="map"/> and wrapped as <see cref="Maybe{T}"/>.</returns>
	public static IEnumerable<Maybe<TReturn>> Map<T, TReturn>(this IEnumerable<T> @this, Func<T, TReturn> map)
	{
		foreach (var item in @this)
		{
			if (item is not null && map(item) is TReturn value)
			{
				yield return value;
			}
		}
	}

	/// <inheritdoc cref="Map{T, TReturn}(IEnumerable{T}, Func{T, TReturn})"/>
	public static async IAsyncEnumerable<Maybe<TReturn>> MapAsync<T, TReturn>(this IEnumerable<T> @this, Func<T, Task<TReturn>> map)
	{
		foreach (var item in @this)
		{
			if (item is not null && await map(item) is TReturn value)
			{
				yield return value;
			}
		}
	}

	/// <inheritdoc cref="Map{T, TReturn}(IEnumerable{T}, Func{T, TReturn})"/>
	public static IAsyncEnumerable<Maybe<TReturn>> MapAsync<T, TReturn>(this IAsyncEnumerable<T> @this, Func<T, TReturn> map) =>
		MapAsync(@this, x => Task.FromResult(map(x)));

	/// <inheritdoc cref="Map{T, TReturn}(IEnumerable{T}, Func{T, TReturn})"/>
	public static async IAsyncEnumerable<Maybe<TReturn>> MapAsync<T, TReturn>(this IAsyncEnumerable<T> @this, Func<T, Task<TReturn>> map)
	{
		await foreach (var item in @this)
		{
			if (item is not null && await map(item) is TReturn value)
			{
				yield return value;
			}
		}
	}
}
