// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Linq;

public static partial class MaybeToResultExtensions
{
	/// <summary>
	/// Combine a <see cref="Maybe{T}"/> with a <see cref="Result{T}"/> in a LINQ query, producing a <see cref="Result{T}"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// For example:
	/// </para>
	/// <code>
	/// from x in maybe
	/// from y in otherResult
	/// select x + y
	/// </code>
	/// <para>
	/// <paramref name="g"/> runs through <see cref="R.Try{T}(Func{T}, R.ExceptionHandler)"/>, so an exception it throws becomes a <see cref="Failure"/>.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <typeparam name="TInner">Interim value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="f">Function that returns the interim <see cref="Result{T}"/>, given the value.</param>
	/// <param name="g">Function that combines the value and the interim value.</param>
	/// <returns>
	/// <see cref="Ok{T}"/> holding the value returned by <paramref name="g"/>; the <see cref="Failure"/> returned by <paramref name="f"/>;
	/// a <see cref="Failure"/> with the message <c>Maybe&lt;{Type}&gt; was 'None'.</c> if <paramref name="this"/> is <see cref="None"/>;
	/// or a <see cref="Failure"/> if <paramref name="g"/> throws or returns <see langword="null"/>.
	/// </returns>
	public static Result<TReturn> SelectMany<T, TInner, TReturn>(this Maybe<T> @this, Func<T, Result<TInner>> f, Func<T, TInner, TReturn> g) =>
		@this switch
		{
			Some<T> x =>
				f(x.Value) switch
				{
					Ok<TInner> y =>
						R.Try(g, x.Value, y.Value, R.DefaultExceptionHandler),

					{ } y =>
						y.PassFailure<TReturn>()
				},

			None =>
				ConvertNoneToFail<T, TReturn>()
		};

	/// <inheritdoc cref="SelectMany{T, TInner, TReturn}(Maybe{T}, Func{T, Result{TInner}}, Func{T, TInner, TReturn})"/>
	public static async Task<Result<TReturn>> SelectMany<T, TInner, TReturn>(this Maybe<T> @this, Func<T, Task<Result<TInner>>> f, Func<T, TInner, TReturn> g) =>
		@this switch
		{
			Some<T> x =>
				await f(x.Value) switch
				{
					Ok<TInner> y =>
						R.Try(g, x.Value, y.Value, R.DefaultExceptionHandler),

					{ } y =>
						y.PassFailure<TReturn>()
				},

			None =>
				ConvertNoneToFail<T, TReturn>()
		};

	/// <inheritdoc cref="SelectMany{T, TInner, TReturn}(Maybe{T}, Func{T, Result{TInner}}, Func{T, TInner, TReturn})"/>
	public static async Task<Result<TReturn>> SelectMany<T, TInner, TReturn>(this Task<Maybe<T>> @this, Func<T, Result<TInner>> f, Func<T, TInner, TReturn> g) =>
		await @this switch
		{
			Some<T> x =>
				f(x.Value) switch
				{
					Ok<TInner> y =>
						R.Try(g, x.Value, y.Value, R.DefaultExceptionHandler),

					{ } y =>
						y.PassFailure<TReturn>()
				},

			None =>
				ConvertNoneToFail<T, TReturn>()
		};

	/// <inheritdoc cref="SelectMany{T, TInner, TReturn}(Maybe{T}, Func{T, Result{TInner}}, Func{T, TInner, TReturn})"/>
	public static async Task<Result<TReturn>> SelectMany<T, TInner, TReturn>(this Task<Maybe<T>> @this, Func<T, Task<Result<TInner>>> f, Func<T, TInner, TReturn> g) =>
		await @this switch
		{
			Some<T> x =>
				await f(x.Value) switch
				{
					Ok<TInner> y =>
						R.Try(g, x.Value, y.Value, R.DefaultExceptionHandler),

					{ } y =>
						y.PassFailure<TReturn>()
				},

			None =>
				ConvertNoneToFail<T, TReturn>()
		};
}
