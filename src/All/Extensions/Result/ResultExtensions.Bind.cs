// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ResultExtensions
{
	/// <summary>
	/// Run <paramref name="f"/> with the value when <paramref name="this"/> is <see cref="Ok{T}"/>.
	/// </summary>
	/// <remarks>
	/// <paramref name="f"/> returns a <see cref="Result{T}"/>, so it is expected to handle its own exceptions and
	/// return a <see cref="Failure"/>: they are not caught here. Use <see cref="Map{T, TReturn}(Result{T}, Func{T, TReturn})"/>
	/// for a function that returns a plain value and may throw.
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="f">Function that converts the value to a <see cref="Result{T}"/>.</param>
	/// <returns>The <see cref="Result{T}"/> returned by <paramref name="f"/>, or the <see cref="Failure"/> held by <paramref name="this"/>.</returns>
	public static Result<TReturn> Bind<T, TReturn>(this Result<T> @this, Func<T, Result<TReturn>> f) =>
		@this switch
		{
			Ok<T> x =>
				f(x.Value),

			// a discard, not `Failure =>`: a Failure arm makes the JIT set up a 40-byte Failure local on every call,
			// even on the Ok path (Result Chain measured 28.8 ns with it, 17.7 ns without)
			_ =>
				@this.PassFailure<TReturn>()
		};

	/// <inheritdoc cref="Bind{T, TReturn}(Result{T}, Func{T, Result{TReturn}})"/>
	public static async Task<Result<TReturn>> BindAsync<T, TReturn>(this Result<T> @this, Func<T, Task<Result<TReturn>>> f) =>
		@this switch
		{
			Ok<T> x =>
				await f(x.Value),

			// a discard, not `Failure =>`: a Failure arm makes the JIT set up a 40-byte Failure local on every call,
			// even on the Ok path (Result Chain measured 28.8 ns with it, 17.7 ns without)
			_ =>
				@this.PassFailure<TReturn>()
		};

	/// <inheritdoc cref="Bind{T, TReturn}(Result{T}, Func{T, Result{TReturn}})"/>
	public static async Task<Result<TReturn>> BindAsync<T, TReturn>(this Task<Result<T>> @this, Func<T, Result<TReturn>> f) =>
		(await @this).Bind(f);

	/// <inheritdoc cref="Bind{T, TReturn}(Result{T}, Func{T, Result{TReturn}})"/>
	public static async Task<Result<TReturn>> BindAsync<T, TReturn>(this Task<Result<T>> @this, Func<T, Task<Result<TReturn>>> f) =>
		await (await @this).BindAsync(f);
}
