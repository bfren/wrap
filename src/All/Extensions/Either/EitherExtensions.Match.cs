// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class EitherExtensions
{
	/// <inheritdoc cref="E.Match{TLeft, TRight}(Either{TLeft, TRight}, Action{TLeft}, Action{TRight})"/>
	public static void Match<TLeft, TRight>(this Either<TLeft, TRight> @this, Action<TLeft> fLeft, Action<TRight> fRight) =>
		E.Match(@this, fLeft, fRight);

	/// <inheritdoc cref="E.Match{TLeft, TRight}(Either{TLeft, TRight}, Action{TLeft}, Action{TRight})"/>
	public static Task MatchAsync<TLeft, TRight>(this Either<TLeft, TRight> @this, Func<TLeft, Task> fLeft, Func<TRight, Task> fRight) =>
		E.MatchAsync(@this, fLeft, fRight);

	/// <inheritdoc cref="E.Match{TLeft, TRight}(Either{TLeft, TRight}, Action{TLeft}, Action{TRight})"/>
	public static Task MatchAsync<TLeft, TRight>(this Task<Either<TLeft, TRight>> @this, Func<TLeft, Task> fLeft, Func<TRight, Task> fRight) =>
		E.MatchAsync(@this, fLeft, fRight);

	/// <inheritdoc cref="E.Match{TLeft, TRight, TReturn}(Either{TLeft, TRight}, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static TReturn Match<TLeft, TRight, TReturn>(this Either<TLeft, TRight> @this, Func<TLeft, TReturn> fLeft, Func<TRight, TReturn> fRight) =>
		E.Match(@this, fLeft, fRight);

	/// <inheritdoc cref="E.Match{TLeft, TRight, TReturn}(Either{TLeft, TRight}, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static Task<TReturn> MatchAsync<TLeft, TRight, TReturn>(this Either<TLeft, TRight> @this, Func<TLeft, TReturn> fLeft, Func<TRight, Task<TReturn>> fRight) =>
		E.MatchAsync(@this, fLeft, fRight);

	/// <inheritdoc cref="E.Match{TLeft, TRight, TReturn}(Either{TLeft, TRight}, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static Task<TReturn> MatchAsync<TLeft, TRight, TReturn>(this Either<TLeft, TRight> @this, Func<TLeft, Task<TReturn>> fLeft, Func<TRight, TReturn> fRight) =>
		E.MatchAsync(@this, fLeft, fRight);

	/// <inheritdoc cref="E.Match{TLeft, TRight, TReturn}(Either{TLeft, TRight}, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static Task<TReturn> MatchAsync<TLeft, TRight, TReturn>(this Either<TLeft, TRight> @this, Func<TLeft, Task<TReturn>> fLeft, Func<TRight, Task<TReturn>> fRight) =>
		E.MatchAsync(@this, fLeft, fRight);

	/// <inheritdoc cref="E.Match{TLeft, TRight, TReturn}(Either{TLeft, TRight}, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static Task<TReturn> MatchAsync<TLeft, TRight, TReturn>(this Task<Either<TLeft, TRight>> @this, Func<TLeft, TReturn> fLeft, Func<TRight, TReturn> fRight) =>
		E.MatchAsync(@this, fLeft, fRight);

	/// <inheritdoc cref="E.Match{TLeft, TRight, TReturn}(Either{TLeft, TRight}, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static Task<TReturn> MatchAsync<TLeft, TRight, TReturn>(this Task<Either<TLeft, TRight>> @this, Func<TLeft, TReturn> fLeft, Func<TRight, Task<TReturn>> fRight) =>
		E.MatchAsync(@this, fLeft, fRight);

	/// <inheritdoc cref="E.Match{TLeft, TRight, TReturn}(Either{TLeft, TRight}, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static Task<TReturn> MatchAsync<TLeft, TRight, TReturn>(this Task<Either<TLeft, TRight>> @this, Func<TLeft, Task<TReturn>> fLeft, Func<TRight, TReturn> fRight) =>
		E.MatchAsync(@this, fLeft, fRight);

	/// <inheritdoc cref="E.Match{TLeft, TRight, TReturn}(Either{TLeft, TRight}, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static Task<TReturn> MatchAsync<TLeft, TRight, TReturn>(this Task<Either<TLeft, TRight>> @this, Func<TLeft, Task<TReturn>> fLeft, Func<TRight, Task<TReturn>> fRight) =>
		E.MatchAsync(@this, fLeft, fRight);
}
