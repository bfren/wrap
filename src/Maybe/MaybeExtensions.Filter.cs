// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Run <paramref name="predicate"/> when <paramref name="this"/> is <see cref="Some{T}"/>
	/// </summary>
	/// <seealso cref="Linq.MaybeExtensions.Where{T}(Maybe{T}, Func{T, bool})"/>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="predicate">Function to detemine whether or not the value of <paramref name="this"/> should be returned.</param>
	/// <returns>Value of <paramref name="this"/> if <paramref name="predicate"/> returns true, or <see cref="None"/>.</returns>
	public static Maybe<T> Filter<T>(this Maybe<T> @this, Func<T, bool> predicate) =>
		@this.Bind(x => predicate(x) switch
		{
			false =>
				M.None,

			true =>
				M.Wrap(x)
		});

	/// <inheritdoc cref="Filter{T}(Maybe{T}, Func{T, bool})"/>
	public static Task<Maybe<T>> FilterAsync<T>(this Maybe<T> @this, Func<T, Task<bool>> predicate) =>
		@this.BindAsync(async x => await predicate(x) switch
		{
			false =>
				M.None,

			true =>
				M.Wrap(x)
		});

	/// <inheritdoc cref="Filter{T}(Maybe{T}, Func{T, bool})"/>
	public static Task<Maybe<T>> FilterAsync<T>(this Task<Maybe<T>> @this, Func<T, bool> predicate) =>
		@this.BindAsync(x => predicate(x) switch
		{
			false =>
				M.None,

			true =>
				M.Wrap(x)
		});

	/// <inheritdoc cref="Filter{T}(Maybe{T}, Func{T, bool})"/>
	public static Task<Maybe<T>> FilterAsync<T>(this Task<Maybe<T>> @this, Func<T, Task<bool>> predicate) =>
		@this.BindAsync(async x => await predicate(x) switch
		{
			false =>
				M.None,

			true =>
				M.Wrap(x)
		});
}
