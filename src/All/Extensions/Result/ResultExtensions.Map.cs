// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ResultExtensions
{
	/// <inheritdoc cref="Map{T, TReturn}(Result{T}, Func{T, TReturn}, R.ExceptionHandler)"/>
	public static Result<TReturn> Map<T, TReturn>(this Result<T> @this, Func<T, TReturn> f) =>
		Map(@this, f, R.DefaultExceptionHandler);

	/// <summary>
	/// Convert the value with <paramref name="f"/> when <paramref name="this"/> is <see cref="Ok{T}"/>, and wrap the result.
	/// </summary>
	/// <remarks>
	/// <para>
	/// <paramref name="f"/> returns a plain value, so it runs through <see cref="R.Try{T}(Func{T}, R.ExceptionHandler)"/>:
	/// an exception it throws becomes the <see cref="Failure"/> returned by the exception handler
	/// (<see cref="R.DefaultExceptionHandler"/> for the overloads without <c>e</c>).
	/// A <see langword="null"/> result also becomes a <see cref="Failure"/>.
	/// </para>
	/// </remarks>
	/// <seealso cref="Bind{T, TReturn}(Result{T}, Func{T, Result{TReturn}})"/>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="f">Function that converts the value.</param>
	/// <param name="e">Exception handler, run if <paramref name="f"/> throws.</param>
	/// <returns>
	/// <see cref="Ok{T}"/> holding the value returned by <paramref name="f"/>; the <see cref="Failure"/> held by
	/// <paramref name="this"/>; or a <see cref="Failure"/> if <paramref name="f"/> throws or returns <see langword="null"/>.
	/// </returns>
	public static Result<TReturn> Map<T, TReturn>(this Result<T> @this, Func<T, TReturn> f, R.ExceptionHandler e) =>
		@this switch
		{
			Ok<T> x =>
				R.Try(f, x.Value, e),

			// a discard, not `Failure =>`: a Failure arm makes the JIT set up a 40-byte Failure local on every call,
			// even on the Ok path (Result Chain measured 28.8 ns with it, 17.7 ns without)
			_ =>
				@this.PassFailure<TReturn>()
		};

	/// <inheritdoc cref="Map{T, TReturn}(Result{T}, Func{T, TReturn}, R.ExceptionHandler)"/>
	public static Task<Result<TReturn>> MapAsync<T, TReturn>(this Result<T> @this, Func<T, Task<TReturn>> f) =>
		MapAsync(@this, f, R.DefaultExceptionHandler);

	/// <inheritdoc cref="Map{T, TReturn}(Result{T}, Func{T, TReturn}, R.ExceptionHandler)"/>
	public static Task<Result<TReturn>> MapAsync<T, TReturn>(this Result<T> @this, Func<T, Task<TReturn>> f, R.ExceptionHandler e) =>
		@this switch
		{
			Ok<T> x =>
				R.TryAsync(f, x.Value, e),

			// a discard, not `Failure =>`: a Failure arm makes the JIT set up a 40-byte Failure local on every call,
			// even on the Ok path (Result Chain measured 28.8 ns with it, 17.7 ns without)
			_ =>
				@this.PassFailure<TReturn>().AsTask()
		};

	/// <inheritdoc cref="Map{T, TReturn}(Result{T}, Func{T, TReturn}, R.ExceptionHandler)"/>
	public static Task<Result<TReturn>> MapAsync<T, TReturn>(this Task<Result<T>> @this, Func<T, TReturn> f) =>
		MapAsync(@this, f, R.DefaultExceptionHandler);

	/// <inheritdoc cref="Map{T, TReturn}(Result{T}, Func{T, TReturn}, R.ExceptionHandler)"/>
	public static async Task<Result<TReturn>> MapAsync<T, TReturn>(this Task<Result<T>> @this, Func<T, TReturn> f, R.ExceptionHandler e) =>
		(await @this).Map(f, e);

	/// <inheritdoc cref="Map{T, TReturn}(Result{T}, Func{T, TReturn}, R.ExceptionHandler)"/>
	public static Task<Result<TReturn>> MapAsync<T, TReturn>(this Task<Result<T>> @this, Func<T, Task<TReturn>> f) =>
		MapAsync(@this, f, R.DefaultExceptionHandler);

	/// <inheritdoc cref="Map{T, TReturn}(Result{T}, Func{T, TReturn}, R.ExceptionHandler)"/>
	public static async Task<Result<TReturn>> MapAsync<T, TReturn>(this Task<Result<T>> @this, Func<T, Task<TReturn>> f, R.ExceptionHandler e) =>
		await (await @this).MapAsync(f, e);
}
