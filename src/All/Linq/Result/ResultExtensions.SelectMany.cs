// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Linq;

public static partial class ResultExtensions
{
	/// <summary>
	/// Combine two <see cref="Result{T}"/> objects in a LINQ query.
	/// </summary>
	/// <remarks>
	/// <para>
	/// For example:
	/// </para>
	/// <code>
	/// from x in result
	/// from y in otherResult
	/// select x + y
	/// </code>
	/// <para>
	/// <paramref name="g"/> runs through <see cref="R.Try{T}(Func{T}, R.ExceptionHandler)"/>, so an exception it throws becomes a <see cref="Failure"/>.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <typeparam name="TInner">Interim value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="f">Function that returns the interim <see cref="Result{T}"/>, given the value.</param>
	/// <param name="g">Function that combines the value and the interim value.</param>
	/// <returns>
	/// <see cref="Ok{T}"/> holding the value returned by <paramref name="g"/>; the first <see cref="Failure"/> (from
	/// <paramref name="this"/> or <paramref name="f"/>); or a <see cref="Failure"/> if <paramref name="g"/> throws or returns <see langword="null"/>.
	/// </returns>
	public static Result<TReturn> SelectMany<T, TInner, TReturn>(this Result<T> @this, Func<T, Result<TInner>> f, Func<T, TInner, TReturn> g) =>
		@this switch
		{
			Ok<T> x =>
				f(x.Value) switch
				{
					Ok<TInner> y =>
						R.Try(g, x.Value, y.Value, R.DefaultExceptionHandler),

					{ } y =>
						y.PassFailure<TReturn>()
				},

			{ } x =>
				x.PassFailure<TReturn>()
		};

	/// <inheritdoc cref="SelectMany{T, TInner, TReturn}(Result{T}, Func{T, Result{TInner}}, Func{T, TInner, TReturn})"/>
	public static async Task<Result<TReturn>> SelectMany<T, TInner, TReturn>(this Result<T> @this, Func<T, Task<Result<TInner>>> f, Func<T, TInner, TReturn> g) =>
		@this switch
		{
			Ok<T> x =>
				await f(x.Value) switch
				{
					Ok<TInner> y =>
						R.Try(g, x.Value, y.Value, R.DefaultExceptionHandler),

					{ } y =>
						y.PassFailure<TReturn>()
				},

			{ } x =>
				x.PassFailure<TReturn>()
		};

	/// <inheritdoc cref="SelectMany{T, TInner, TReturn}(Result{T}, Func{T, Result{TInner}}, Func{T, TInner, TReturn})"/>
	public static async Task<Result<TReturn>> SelectMany<T, TInner, TReturn>(this Task<Result<T>> @this, Func<T, Result<TInner>> f, Func<T, TInner, TReturn> g) =>
		await @this switch
		{
			Ok<T> x =>
				f(x.Value) switch
				{
					Ok<TInner> y =>
						R.Try(g, x.Value, y.Value, R.DefaultExceptionHandler),

					{ } y =>
						y.PassFailure<TReturn>()
				},

			{ } x =>
				x.PassFailure<TReturn>()
		};

	/// <inheritdoc cref="SelectMany{T, TInner, TReturn}(Result{T}, Func{T, Result{TInner}}, Func{T, TInner, TReturn})"/>
	public static async Task<Result<TReturn>> SelectMany<T, TInner, TReturn>(this Task<Result<T>> @this, Func<T, Task<Result<TInner>>> f, Func<T, TInner, TReturn> g) =>
		await @this switch
		{
			Ok<T> x =>
				await f(x.Value) switch
				{
					Ok<TInner> y =>
						R.Try(g, x.Value, y.Value, R.DefaultExceptionHandler),

					{ } y =>
						y.PassFailure<TReturn>()
				},

			{ } x =>
				x.PassFailure<TReturn>()
		};
}
