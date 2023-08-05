// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;
using Monadic.Exceptions;

namespace Monadic;

public static partial class ResultExtensions
{
	public static Result<T> Filter<T>(this Result<T> @this, Func<T, bool> predicate) =>
		@this.Bind(x => predicate(x) switch
		{
			false =>
				R.Err<PredicateFalseException>(),

			true =>
				R.Ok(x)
		});

	public static Task<Result<T>> FilterAsync<T>(this Result<T> @this, Func<T, Task<bool>> predicate) =>
		@this.BindAsync(async x => await predicate(x) switch
		{
			false =>
				R.Err<PredicateFalseException>(),

			true =>
				R.Ok(x)
		});

	public static Task<Result<T>> FilterAsync<T>(this Task<Result<T>> @this, Func<T, bool> predicate) =>
		@this.BindAsync(x => predicate(x) switch
		{
			false =>
				R.Err<PredicateFalseException>(),

			true =>
				R.Ok(x)
		});

	public static Task<Result<T>> FilterAsync<T>(this Task<Result<T>> @this, Func<T, Task<bool>> predicate) =>
		@this.BindAsync(async x => await predicate(x) switch
		{
			false =>
				R.Err<PredicateFalseException>(),

			true =>
				R.Ok(x)
		});
}
