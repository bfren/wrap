// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ResultExtensions
{
	/// <summary>
	/// Convert the value with <paramref name="f"/> when <paramref name="this"/> is <see cref="Ok{T}"/> and <paramref name="fTest"/> returns <see langword="true"/>.
	/// </summary>
	/// <remarks>
	/// <paramref name="f"/> runs through <see cref="R.Try{T}(Func{T}, R.ExceptionHandler)"/> with
	/// <see cref="R.DefaultExceptionHandler"/>, so an exception it throws becomes a <see cref="Failure"/>.
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="fTest">Test to run on the value.</param>
	/// <param name="f">Function that converts the value, run when <paramref name="fTest"/> returns <see langword="true"/>.</param>
	/// <returns>
	/// <see cref="Ok{T}"/> holding the value returned by <paramref name="f"/>; the <see cref="Failure"/> held by <paramref name="this"/>;
	/// a <see cref="Failure"/> with the message <c>Test returned false.</c> if <paramref name="fTest"/> returns <see langword="false"/>;
	/// or a <see cref="Failure"/> if <paramref name="f"/> throws or returns <see langword="null"/>.
	/// </returns>
	public static Result<TReturn> MapIf<T, TReturn>(this Result<T> @this, Func<T, bool> fTest, Func<T, TReturn> f) =>
		@this switch
		{
			Ok<T> x =>
				fTest(x.Value) switch
				{
					true =>
						R.Try(f, x.Value, R.DefaultExceptionHandler),

					false =>
						R.Fail(C.TestFalseMessage).Ctx(nameof(ResultExtensions), nameof(MapIf))
				},

			{ } x =>
				x.PassFailure<TReturn>()
		};

	/// <inheritdoc cref="MapIf{T, TReturn}(Result{T}, Func{T, bool}, Func{T, TReturn})"/>
	public static async Task<Result<TReturn>> MapIfAsync<T, TReturn>(this Result<T> @this, Func<T, bool> fTest, Func<T, Task<TReturn>> f) =>
		@this switch
		{
			Ok<T> x =>
				fTest(x.Value) switch
				{
					true =>
						await R.TryAsync(f, x.Value, R.DefaultExceptionHandler),

					false =>
						R.Fail(C.TestFalseMessage).Ctx(nameof(ResultExtensions), nameof(MapIf))
				},

			{ } x =>
				x.PassFailure<TReturn>()
		};

	/// <inheritdoc cref="MapIf{T, TReturn}(Result{T}, Func{T, bool}, Func{T, TReturn})"/>
	public static async Task<Result<TReturn>> MapIfAsync<T, TReturn>(this Task<Result<T>> @this, Func<T, bool> fTest, Func<T, TReturn> f) =>
		await @this switch
		{
			Ok<T> x =>
				fTest(x.Value) switch
				{
					true =>
						R.Try(f, x.Value, R.DefaultExceptionHandler),

					false =>
						R.Fail(C.TestFalseMessage).Ctx(nameof(ResultExtensions), nameof(MapIf))
				},

			{ } x =>
				x.PassFailure<TReturn>()
		};

	/// <inheritdoc cref="MapIf{T, TReturn}(Result{T}, Func{T, bool}, Func{T, TReturn})"/>
	public static async Task<Result<TReturn>> MapIfAsync<T, TReturn>(this Task<Result<T>> @this, Func<T, bool> fTest, Func<T, Task<TReturn>> f) =>
		await @this switch
		{
			Ok<T> x =>
				fTest(x.Value) switch
				{
					true =>
						await R.TryAsync(f, x.Value, R.DefaultExceptionHandler),

					false =>
						R.Fail(C.TestFalseMessage).Ctx(nameof(ResultExtensions), nameof(MapIf))
				},

			{ } x =>
				x.PassFailure<TReturn>()
		};
}
