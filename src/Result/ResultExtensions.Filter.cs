// Monads: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;
using Monads.Exceptions;

namespace Monads;

public static partial class ResultExtensions
{
	/// <summary>
	/// Run <paramref name="predicate"/> when <paramref name="this"/> is <see cref="Ok{T}"/>
	/// </summary>
	/// <seealso cref="Linq.ResultExtensions.Where{T}(Result{T}, Func{T, bool})"/>
	/// <typeparam name="T">Result value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="predicate">Function to detemine whether or not the value of <paramref name="this"/> should be returned.</param>
	/// <returns>Value of <paramref name="this"/> if <paramref name="predicate"/> returns true, or <see cref="Err"/>.</returns>
	public static Result<T> Filter<T>(this Result<T> @this, Func<T, bool> predicate) =>
		@this.Bind(x => predicate(x) switch
		{
			false =>
				R.Err<PredicateFalseException>(),

			true =>
				R.Wrap(x)
		});

	/// <inheritdoc cref="Filter{T}(Result{T}, Func{T, bool})"/>
	public static Task<Result<T>> FilterAsync<T>(this Result<T> @this, Func<T, Task<bool>> predicate) =>
		@this.BindAsync(async x => await predicate(x) switch
		{
			false =>
				R.Err<PredicateFalseException>(),

			true =>
				R.Wrap(x)
		});

	/// <inheritdoc cref="Filter{T}(Result{T}, Func{T, bool})"/>
	public static Task<Result<T>> FilterAsync<T>(this Task<Result<T>> @this, Func<T, bool> predicate) =>
		@this.BindAsync(x => predicate(x) switch
		{
			false =>
				R.Err<PredicateFalseException>(),

			true =>
				R.Wrap(x)
		});

	/// <inheritdoc cref="Filter{T}(Result{T}, Func{T, bool})"/>
	public static Task<Result<T>> FilterAsync<T>(this Task<Result<T>> @this, Func<T, Task<bool>> predicate) =>
		@this.BindAsync(async x => await predicate(x) switch
		{
			false =>
				R.Err<PredicateFalseException>(),

			true =>
				R.Wrap(x)
		});
}
