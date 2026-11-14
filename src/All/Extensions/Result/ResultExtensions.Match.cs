// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ResultExtensions
{
	/// <summary>
	/// Run an action based on the value of <paramref name="this"/> - see <see cref="R.Match{T}(Result{T}, Action{Failure}, Action{T})"/>.
	/// </summary>
	/// <remarks>
	/// This is shorthand for a <see langword="switch"/> over the <see cref="Ok{T}"/> and <see cref="Failure"/> cases
	/// that passes the unwrapped value to <paramref name="fOk"/>. Both approaches make you handle both cases:
	/// here through the required arguments, and in a <see langword="switch"/> through the compiler's exhaustiveness check.
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="fFail">Action to run with the failure when <paramref name="this"/> is a <see cref="Failure"/>.</param>
	/// <param name="fOk">Action to run with the value when <paramref name="this"/> is an <see cref="Ok{T}"/>.</param>
	public static void Match<T>(this Result<T> @this, Action<Failure> fFail, Action<T> fOk) =>
		R.Match(@this, fFail, fOk);

	/// <inheritdoc cref="Match{T}(Result{T}, Action{Failure}, Action{T})"/>
	public static Task MatchAsync<T>(this Result<T> @this, Func<Failure, Task> fFail, Func<T, Task> fOk) =>
		R.MatchAsync(@this, fFail, fOk);

	/// <inheritdoc cref="Match{T}(Result{T}, Action{Failure}, Action{T})"/>
	public static Task MatchAsync<T>(this Task<Result<T>> @this, Func<Failure, Task> fFail, Func<T, Task> fOk) =>
		R.MatchAsync(@this, fFail, fOk);

	/// <summary>
	/// Run a function based on the value of <paramref name="this"/> and return its value - see
	/// <see cref="R.Match{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, TReturn})"/>.
	/// </summary>
	/// <remarks>
	/// <inheritdoc cref="Match{T}(Result{T}, Action{Failure}, Action{T})" path="/remarks"/>
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="fFail">Function to run with the failure when <paramref name="this"/> is a <see cref="Failure"/>.</param>
	/// <param name="fOk">Function to run with the value when <paramref name="this"/> is an <see cref="Ok{T}"/>.</param>
	/// <returns>Value returned by <paramref name="fFail"/> or <paramref name="fOk"/>.</returns>
	public static TReturn Match<T, TReturn>(this Result<T> @this, Func<Failure, TReturn> fFail, Func<T, TReturn> fOk) =>
		R.Match(@this, fFail, fOk);

	/// <inheritdoc cref="Match{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Result<T> @this, Func<Failure, TReturn> fFail, Func<T, Task<TReturn>> fOk) =>
		R.MatchAsync(@this, fFail, fOk);

	/// <inheritdoc cref="Match{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Result<T> @this, Func<Failure, Task<TReturn>> fFail, Func<T, TReturn> fOk) =>
		R.MatchAsync(@this, fFail, fOk);

	/// <inheritdoc cref="Match{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Result<T> @this, Func<Failure, Task<TReturn>> fFail, Func<T, Task<TReturn>> fOk) =>
		R.MatchAsync(@this, fFail, fOk);

	/// <inheritdoc cref="Match{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Task<Result<T>> @this, Func<Failure, TReturn> fFail, Func<T, TReturn> fOk) =>
		R.MatchAsync(@this, fFail, fOk);

	/// <inheritdoc cref="Match{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Task<Result<T>> @this, Func<Failure, TReturn> fFail, Func<T, Task<TReturn>> fOk) =>
		R.MatchAsync(@this, fFail, fOk);

	/// <inheritdoc cref="Match{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Task<Result<T>> @this, Func<Failure, Task<TReturn>> fFail, Func<T, TReturn> fOk) =>
		R.MatchAsync(@this, fFail, fOk);

	/// <inheritdoc cref="Match{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Task<Result<T>> @this, Func<Failure, Task<TReturn>> fFail, Func<T, Task<TReturn>> fOk) =>
		R.MatchAsync(@this, fFail, fOk);
}
