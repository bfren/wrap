// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class EitherExtensions
{
	/// <inheritdoc cref="E.Match{TEither, TLeft, TRight}(TEither, Action{TLeft}, Action{TRight})"/>
	public static void Match<TEither, TLeft, TRight>(this TEither @this, Action<TLeft> fLeft, Action<TRight> fRight)
		where TEither : IEither<TEither, TLeft, TRight> =>
		E.Match(@this, fLeft, fRight);

	/// <inheritdoc cref="E.Match{TEither, TLeft, TRight}(TEither, Action{TLeft}, Action{TRight})"/>
	public static Task MatchAsync<TEither, TLeft, TRight>(this TEither @this, Func<TLeft, Task> fLeft, Func<TRight, Task> fRight)
		where TEither : IEither<TEither, TLeft, TRight> =>
		E.MatchAsync(@this, fLeft, fRight);

	/// <inheritdoc cref="E.Match{TEither, TLeft, TRight}(TEither, Action{TLeft}, Action{TRight})"/>
	public static Task MatchAsync<TEither, TLeft, TRight>(this Task<TEither> @this, Func<TLeft, Task> fLeft, Func<TRight, Task> fRight)
		where TEither : IEither<TEither, TLeft, TRight> =>
		E.MatchAsync(@this, fLeft, fRight);

	/// <inheritdoc cref="E.Match{TEither, TLeft, TRight, TReturn}(TEither, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static TReturn Match<TEither, TLeft, TRight, TReturn>(this TEither @this, Func<TLeft, TReturn> fLeft, Func<TRight, TReturn> fRight)
		where TEither : IEither<TEither, TLeft, TRight> =>
		E.Match(@this, fLeft, fRight);

	/// <inheritdoc cref="E.Match{TEither, TLeft, TRight, TReturn}(TEither, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static Task<TReturn> MatchAsync<TEither, TLeft, TRight, TReturn>(this TEither @this, Func<TLeft, TReturn> fLeft, Func<TRight, Task<TReturn>> fRight)
		where TEither : IEither<TEither, TLeft, TRight> =>
		E.MatchAsync(@this, fLeft, fRight);

	/// <inheritdoc cref="E.Match{TEither, TLeft, TRight, TReturn}(TEither, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static Task<TReturn> MatchAsync<TEither, TLeft, TRight, TReturn>(this TEither @this, Func<TLeft, Task<TReturn>> fLeft, Func<TRight, TReturn> fRight)
		where TEither : IEither<TEither, TLeft, TRight> =>
		E.MatchAsync(@this, fLeft, fRight);

	/// <inheritdoc cref="E.Match{TEither, TLeft, TRight, TReturn}(TEither, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static Task<TReturn> MatchAsync<TEither, TLeft, TRight, TReturn>(this TEither @this, Func<TLeft, Task<TReturn>> fLeft, Func<TRight, Task<TReturn>> fRight)
		where TEither : IEither<TEither, TLeft, TRight> =>
		E.MatchAsync(@this, fLeft, fRight);

	/// <inheritdoc cref="E.Match{TEither, TLeft, TRight, TReturn}(TEither, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static Task<TReturn> MatchAsync<TEither, TLeft, TRight, TReturn>(this Task<TEither> @this, Func<TLeft, TReturn> fLeft, Func<TRight, TReturn> fRight)
		where TEither : IEither<TEither, TLeft, TRight> =>
		E.MatchAsync(@this, fLeft, fRight);

	/// <inheritdoc cref="E.Match{TEither, TLeft, TRight, TReturn}(TEither, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static Task<TReturn> MatchAsync<TEither, TLeft, TRight, TReturn>(this Task<TEither> @this, Func<TLeft, TReturn> fLeft, Func<TRight, Task<TReturn>> fRight)
		where TEither : IEither<TEither, TLeft, TRight> =>
		E.MatchAsync(@this, fLeft, fRight);

	/// <inheritdoc cref="E.Match{TEither, TLeft, TRight, TReturn}(TEither, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static Task<TReturn> MatchAsync<TEither, TLeft, TRight, TReturn>(this Task<TEither> @this, Func<TLeft, Task<TReturn>> fLeft, Func<TRight, TReturn> fRight)
		where TEither : IEither<TEither, TLeft, TRight> =>
		E.MatchAsync(@this, fLeft, fRight);

	/// <inheritdoc cref="E.Match{TEither, TLeft, TRight, TReturn}(TEither, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static Task<TReturn> MatchAsync<TEither, TLeft, TRight, TReturn>(this Task<TEither> @this, Func<TLeft, Task<TReturn>> fLeft, Func<TRight, Task<TReturn>> fRight)
		where TEither : IEither<TEither, TLeft, TRight> =>
		E.MatchAsync(@this, fLeft, fRight);
}
