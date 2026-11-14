// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class EitherExtensions
{
	/// <summary>
	/// Run an action based on the value of <paramref name="this"/> - see
	/// <see cref="E.Match{TLeft, TRight}(Either{TLeft, TRight}, Action{TLeft}, Action{TRight})"/>.
	/// </summary>
	/// <remarks>
	/// This is shorthand for a <see langword="switch"/> over the <see cref="Left{T}"/> and <see cref="Right{T}"/> cases
	/// that passes the unwrapped value to <paramref name="fLeft"/> or <paramref name="fRight"/>. Both approaches
	/// make you handle both cases: here through the required arguments, and in a <see langword="switch"/> through the
	/// compiler's exhaustiveness check.
	/// </remarks>
	/// <typeparam name="TLeft">Left (error / invalid) value type.</typeparam>
	/// <typeparam name="TRight">Right (correct / valid) value type.</typeparam>
	/// <param name="this">Either object.</param>
	/// <param name="fLeft">Action to run with the value when <paramref name="this"/> is a <see cref="Left{T}"/>.</param>
	/// <param name="fRight">Action to run with the value when <paramref name="this"/> is a <see cref="Right{T}"/>.</param>
	public static void Match<TLeft, TRight>(this Either<TLeft, TRight> @this, Action<TLeft> fLeft, Action<TRight> fRight) =>
		E.Match(@this, fLeft, fRight);

	/// <inheritdoc cref="Match{TLeft, TRight}(Either{TLeft, TRight}, Action{TLeft}, Action{TRight})"/>
	public static Task MatchAsync<TLeft, TRight>(this Either<TLeft, TRight> @this, Func<TLeft, Task> fLeft, Func<TRight, Task> fRight) =>
		E.MatchAsync(@this, fLeft, fRight);

	/// <inheritdoc cref="Match{TLeft, TRight}(Either{TLeft, TRight}, Action{TLeft}, Action{TRight})"/>
	public static Task MatchAsync<TLeft, TRight>(this Task<Either<TLeft, TRight>> @this, Func<TLeft, Task> fLeft, Func<TRight, Task> fRight) =>
		E.MatchAsync(@this, fLeft, fRight);

	/// <summary>
	/// Run a function based on the value of <paramref name="this"/> and return its value - see
	/// <see cref="E.Match{TLeft, TRight, TReturn}(Either{TLeft, TRight}, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>.
	/// </summary>
	/// <remarks>
	/// <inheritdoc cref="Match{TLeft, TRight}(Either{TLeft, TRight}, Action{TLeft}, Action{TRight})" path="/remarks"/>
	/// </remarks>
	/// <typeparam name="TLeft">Left (error / invalid) value type.</typeparam>
	/// <typeparam name="TRight">Right (correct / valid) value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">Either object.</param>
	/// <param name="fLeft">Function to run with the value when <paramref name="this"/> is a <see cref="Left{T}"/>.</param>
	/// <param name="fRight">Function to run with the value when <paramref name="this"/> is a <see cref="Right{T}"/>.</param>
	/// <returns>Value returned by <paramref name="fLeft"/> or <paramref name="fRight"/>.</returns>
	public static TReturn Match<TLeft, TRight, TReturn>(this Either<TLeft, TRight> @this, Func<TLeft, TReturn> fLeft, Func<TRight, TReturn> fRight) =>
		E.Match(@this, fLeft, fRight);

	/// <inheritdoc cref="Match{TLeft, TRight, TReturn}(Either{TLeft, TRight}, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static Task<TReturn> MatchAsync<TLeft, TRight, TReturn>(this Either<TLeft, TRight> @this, Func<TLeft, TReturn> fLeft, Func<TRight, Task<TReturn>> fRight) =>
		E.MatchAsync(@this, fLeft, fRight);

	/// <inheritdoc cref="Match{TLeft, TRight, TReturn}(Either{TLeft, TRight}, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static Task<TReturn> MatchAsync<TLeft, TRight, TReturn>(this Either<TLeft, TRight> @this, Func<TLeft, Task<TReturn>> fLeft, Func<TRight, TReturn> fRight) =>
		E.MatchAsync(@this, fLeft, fRight);

	/// <inheritdoc cref="Match{TLeft, TRight, TReturn}(Either{TLeft, TRight}, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static Task<TReturn> MatchAsync<TLeft, TRight, TReturn>(this Either<TLeft, TRight> @this, Func<TLeft, Task<TReturn>> fLeft, Func<TRight, Task<TReturn>> fRight) =>
		E.MatchAsync(@this, fLeft, fRight);

	/// <inheritdoc cref="Match{TLeft, TRight, TReturn}(Either{TLeft, TRight}, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static Task<TReturn> MatchAsync<TLeft, TRight, TReturn>(this Task<Either<TLeft, TRight>> @this, Func<TLeft, TReturn> fLeft, Func<TRight, TReturn> fRight) =>
		E.MatchAsync(@this, fLeft, fRight);

	/// <inheritdoc cref="Match{TLeft, TRight, TReturn}(Either{TLeft, TRight}, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static Task<TReturn> MatchAsync<TLeft, TRight, TReturn>(this Task<Either<TLeft, TRight>> @this, Func<TLeft, TReturn> fLeft, Func<TRight, Task<TReturn>> fRight) =>
		E.MatchAsync(@this, fLeft, fRight);

	/// <inheritdoc cref="Match{TLeft, TRight, TReturn}(Either{TLeft, TRight}, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static Task<TReturn> MatchAsync<TLeft, TRight, TReturn>(this Task<Either<TLeft, TRight>> @this, Func<TLeft, Task<TReturn>> fLeft, Func<TRight, TReturn> fRight) =>
		E.MatchAsync(@this, fLeft, fRight);

	/// <inheritdoc cref="Match{TLeft, TRight, TReturn}(Either{TLeft, TRight}, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static Task<TReturn> MatchAsync<TLeft, TRight, TReturn>(this Task<Either<TLeft, TRight>> @this, Func<TLeft, Task<TReturn>> fLeft, Func<TRight, Task<TReturn>> fRight) =>
		E.MatchAsync(@this, fLeft, fRight);
}
