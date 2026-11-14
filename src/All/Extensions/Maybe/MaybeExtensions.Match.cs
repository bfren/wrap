// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Run an action based on the value of <paramref name="this"/> - see <see cref="M.Match{T}(Maybe{T}, Action, Action{T})"/>.
	/// </summary>
	/// <remarks>
	/// This is shorthand for a <see langword="switch"/> over the <see cref="Some{T}"/> and <see cref="None"/> cases
	/// that passes the unwrapped value to <paramref name="fSome"/>. Both approaches make you handle both cases:
	/// here through the required arguments, and in a <see langword="switch"/> through the compiler's exhaustiveness check.
	/// </remarks>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="fNone">Action to run when <paramref name="this"/> is <see cref="None"/>.</param>
	/// <param name="fSome">Action to run with the value when <paramref name="this"/> is <see cref="Some{T}"/>.</param>
	public static void Match<T>(this Maybe<T> @this, Action fNone, Action<T> fSome) =>
		M.Match(@this, fNone, fSome);

	/// <inheritdoc cref="Match{T}(Maybe{T}, Action, Action{T})"/>
	public static Task MatchAsync<T>(this Maybe<T> @this, Action fNone, Func<T, Task> fSome) =>
		M.MatchAsync(@this, fNone, fSome);

	/// <inheritdoc cref="Match{T}(Maybe{T}, Action, Action{T})"/>
	public static Task MatchAsync<T>(this Maybe<T> @this, Func<Task> fNone, Action<T> fSome) =>
		M.MatchAsync(@this, fNone, fSome);

	/// <inheritdoc cref="Match{T}(Maybe{T}, Action, Action{T})"/>
	public static Task MatchAsync<T>(this Maybe<T> @this, Func<Task> fNone, Func<T, Task> fSome) =>
		M.MatchAsync(@this, fNone, fSome);

	/// <inheritdoc cref="Match{T}(Maybe{T}, Action, Action{T})"/>
	public static Task MatchAsync<T>(this Task<Maybe<T>> @this, Func<Task> fNone, Func<T, Task> fSome) =>
		M.MatchAsync(@this, fNone, fSome);

	/// <summary>
	/// Run a function based on the value of <paramref name="this"/> and return its value - see
	/// <see cref="M.Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>.
	/// </summary>
	/// <remarks>
	/// <inheritdoc cref="Match{T}(Maybe{T}, Action, Action{T})" path="/remarks"/>
	/// </remarks>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="fNone">Function to run when <paramref name="this"/> is <see cref="None"/>.</param>
	/// <param name="fSome">Function to run with the value when <paramref name="this"/> is <see cref="Some{T}"/>.</param>
	/// <returns>Value returned by <paramref name="fNone"/> or <paramref name="fSome"/>.</returns>
	public static TReturn Match<T, TReturn>(this Maybe<T> @this, Func<TReturn> fNone, Func<T, TReturn> fSome) =>
		M.Match(@this, fNone, fSome);

	/// <inheritdoc cref="Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Maybe<T> @this, Func<TReturn> fNone, Func<T, Task<TReturn>> fSome) =>
		M.MatchAsync(@this, fNone, fSome);

	/// <inheritdoc cref="Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Maybe<T> @this, Func<Task<TReturn>> fNone, Func<T, TReturn> fSome) =>
		M.MatchAsync(@this, fNone, fSome);

	/// <inheritdoc cref="Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Maybe<T> @this, Func<Task<TReturn>> fNone, Func<T, Task<TReturn>> fSome) =>
		M.MatchAsync(@this, fNone, fSome);

	/// <inheritdoc cref="Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<TReturn> fNone, Func<T, TReturn> fSome) =>
		M.MatchAsync(@this, fNone, fSome);

	/// <inheritdoc cref="Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<TReturn> fNone, Func<T, Task<TReturn>> fSome) =>
		M.MatchAsync(@this, fNone, fSome);

	/// <inheritdoc cref="Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<Task<TReturn>> fNone, Func<T, TReturn> fSome) =>
		M.MatchAsync(@this, fNone, fSome);

	/// <inheritdoc cref="Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<Task<TReturn>> fNone, Func<T, Task<TReturn>> fSome) =>
		M.MatchAsync(@this, fNone, fSome);
}
