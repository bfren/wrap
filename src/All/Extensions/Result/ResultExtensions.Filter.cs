// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ResultExtensions
{
	/// <summary>
	/// Keep the value of <paramref name="this"/> only if <paramref name="fTest"/> returns <see langword="true"/>.
	/// </summary>
	/// <seealso cref="Linq.ResultExtensions.Where{T}(Result{T}, Func{T, bool})"/>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="fTest">Test to run on the value.</param>
	/// <returns>
	/// <paramref name="this"/> if it is a <see cref="Failure"/> or <paramref name="fTest"/> returns <see langword="true"/>,
	/// otherwise a <see cref="Failure"/> with the message <c>Test returned false.</c>.
	/// </returns>
	public static Result<T> Filter<T>(this Result<T> @this, Func<T, bool> fTest) =>
		@this switch
		{
			Ok<T> x when fTest(x.Value) =>
				x,

			Ok<T> =>
				R.Fail(C.TestFalseMessage).Ctx(nameof(ResultExtensions), nameof(Filter)),

			{ } x =>
				x
		};

	/// <inheritdoc cref="Filter{T}(Result{T}, Func{T, bool})"/>
	public static async Task<Result<T>> FilterAsync<T>(this Result<T> @this, Func<T, Task<bool>> fTest) =>
		@this switch
		{
			Ok<T> x when await fTest(x.Value) =>
				x,

			Ok<T> =>
				R.Fail(C.TestFalseMessage).Ctx(nameof(ResultExtensions), nameof(FilterAsync)),

			{ } x =>
				x
		};

	/// <inheritdoc cref="Filter{T}(Result{T}, Func{T, bool})"/>
	public static async Task<Result<T>> FilterAsync<T>(this Task<Result<T>> @this, Func<T, bool> fTest) =>
		await @this switch
		{
			Ok<T> x when fTest(x.Value) =>
				x,

			Ok<T> =>
				R.Fail(C.TestFalseMessage).Ctx(nameof(ResultExtensions), nameof(FilterAsync)),

			{ } x =>
				x
		};

	/// <inheritdoc cref="Filter{T}(Result{T}, Func{T, bool})"/>
	public static async Task<Result<T>> FilterAsync<T>(this Task<Result<T>> @this, Func<T, Task<bool>> fTest) =>
		await @this switch
		{
			Ok<T> x when await fTest(x.Value) =>
				x,

			Ok<T> =>
				R.Fail(C.TestFalseMessage).Ctx(nameof(ResultExtensions), nameof(FilterAsync)),

			{ } x =>
				x
		};
}
