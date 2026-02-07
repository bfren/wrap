// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;
using Wrap.Exceptions;

namespace Wrap.Extensions;

public static partial class ResultExtensions
{
	/// <summary>
	/// Run <paramref name="fTest"/> when <paramref name="this"/> is <see cref="Ok{T}"/>
	/// </summary>
	/// <seealso cref="Linq.ResultExtensions.Where{T}(Result{T}, Func{T, bool})"/>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="fTest">Function to detemine whether or not the value of <paramref name="this"/> should be returned.</param>
	/// <returns>Value of <paramref name="this"/> if <paramref name="fTest"/> returns true, or <see cref="Failure"/>.</returns>
	public static Result<T> Filter<T>(this Result<T> @this, Func<T, bool> fTest) =>
		@this.Bind(x => fTest(x) switch
		{
			false =>
				R.Fail<TestFalseException>(),

			true =>
				R.Wrap(x)
		});

	/// <inheritdoc cref="Filter{T}(Result{T}, Func{T, bool})"/>
	public static Task<Result<T>> FilterAsync<T>(this Result<T> @this, Func<T, Task<bool>> fTest) =>
		@this.BindAsync(async x => await fTest(x) switch
		{
			false =>
				R.Fail<TestFalseException>(),

			true =>
				R.Wrap(x)
		});

	/// <inheritdoc cref="Filter{T}(Result{T}, Func{T, bool})"/>
	public static Task<Result<T>> FilterAsync<T>(this Task<Result<T>> @this, Func<T, bool> fTest) =>
		@this.BindAsync(x => fTest(x) switch
		{
			false =>
				R.Fail<TestFalseException>(),

			true =>
				R.Wrap(x)
		});

	/// <inheritdoc cref="Filter{T}(Result{T}, Func{T, bool})"/>
	public static Task<Result<T>> FilterAsync<T>(this Task<Result<T>> @this, Func<T, Task<bool>> fTest) =>
		@this.BindAsync(async x => await fTest(x) switch
		{
			false =>
				R.Fail<TestFalseException>(),

			true =>
				R.Wrap(x)
		});
}
