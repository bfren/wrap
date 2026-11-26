// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Convert a <see cref="Maybe{T}"/> to a <see cref="Result{T}"/>, turning <see cref="None"/> into a <see cref="Failure"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="className">Failure context class.</param>
	/// <param name="functionName">Failure context function.</param>
	/// <returns>
	/// <see cref="Ok{T}"/> holding the value, or a <see cref="Failure"/> with the message <c>Maybe&lt;{Type}&gt; was 'None'.</c>
	/// and the context <c>{className}.{functionName}()</c> if <paramref name="this"/> is <see cref="None"/>.
	/// </returns>
	public static Result<T> ToResult<T>(this Maybe<T> @this, string className, string functionName) =>
		@this switch
		{
			Some<T> x =>
				x.Value,

			None =>
				R.Fail(C.NoneFailureMessage, typeof(T).Name).WithContext(className, functionName)
		};

	/// <inheritdoc cref="ToResult{T}(Maybe{T}, string, string)"/>
	public static async Task<Result<T>> ToResultAsync<T>(this Task<Maybe<T>> @this, string className, string functionName) =>
		ToResult(await @this, className, functionName);

	/// <summary>
	/// Convert a <see cref="Maybe{T}"/> to a <see cref="Result{T}"/>, using <paramref name="fNone"/> when it is <see cref="None"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="fNone">Returns the result to use when <paramref name="this"/> is <see cref="None"/>.</param>
	/// <remarks>
	/// Exceptions thrown by <paramref name="fNone"/> are not caught: it returns a <see cref="Result{T}"/>, so it should handle its own.
	/// </remarks>
	/// <returns>
	/// <see cref="Ok{T}"/> holding the value, or the result returned by <paramref name="fNone"/>.
	/// </returns>
	public static Result<T> ToResult<T>(this Maybe<T> @this, Func<Result<T>> fNone) =>
		M.Match(@this,
			fNone: fNone,
			fSome: R.Wrap
		);

	/// <inheritdoc cref="ToResult{T}(Maybe{T}, Func{Result{T}})"/>
	public static Task<Result<T>> ToResultAsync<T>(this Maybe<T> @this, Func<Task<Result<T>>> fNone) =>
		M.MatchAsync(@this,
			fNone: fNone,
			fSome: R.Wrap
		);

	/// <inheritdoc cref="ToResult{T}(Maybe{T}, Func{Result{T}})"/>
	public static async Task<Result<T>> ToResultAsync<T>(this Task<Maybe<T>> @this, Func<Result<T>> fNone) =>
		ToResult(await @this, fNone);

	/// <inheritdoc cref="ToResult{T}(Maybe{T}, Func{Result{T}})"/>
	public static Task<Result<T>> ToResultAsync<T>(this Task<Maybe<T>> @this, Func<Task<Result<T>>> fNone) =>
		M.MatchAsync(@this,
			fNone: fNone,
			fSome: R.Wrap
		);
}
