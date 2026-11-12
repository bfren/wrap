// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;
using Wrap.Exceptions;

namespace Wrap;

public static partial class E
{
	#region Without Return Value

	/// <summary>
	/// Run an action based on the value of <paramref name="either"/>.
	/// </summary>
	/// <remarks>
	/// Works with any <see cref="IEither{TEither, TLeft, TRight}"/>, for example <see cref="Either{TLeft, TRight}"/>,
	/// <c>Maybe&lt;T&gt;</c> or <c>Result&lt;T&gt;</c>, by calling <see cref="IEither{TLeft, TRight}.Match{TReturn}"/>.
	/// For an <see cref="Either{TLeft, TRight}"/> a <c>switch</c> over <see cref="Left{T}"/> and <see cref="Right{T}"/>
	/// does the same, and the compiler checks that both cases are handled.
	/// </remarks>
	/// <typeparam name="TEither">Either implementation type.</typeparam>
	/// <typeparam name="TLeft">Left (error / invalid) value type.</typeparam>
	/// <typeparam name="TRight">Right (correct / valid) value type.</typeparam>
	/// <param name="either">Either object.</param>
	/// <param name="fLeft">Action to run with the value when <paramref name="either"/> holds a left value.</param>
	/// <param name="fRight">Action to run with the value when <paramref name="either"/> holds a right value.</param>
	/// <exception cref="NullEitherException"><paramref name="either"/> is <see langword="null"/>.</exception>
	public static void Match<TEither, TLeft, TRight>(TEither either, Action<TLeft> fLeft, Action<TRight> fRight)
		where TEither : IEither<TEither, TLeft, TRight> =>
		_ = Match<TEither, TLeft, TRight, bool>(either,
			fLeft: x => { fLeft(x); return true; },
			fRight: x => { fRight(x); return true; }
		);

	/// <inheritdoc cref="Match{TEither, TLeft, TRight}(TEither, Action{TLeft}, Action{TRight})"/>
	public static async Task MatchAsync<TEither, TLeft, TRight>(TEither either, Action<TLeft> fLeft, Func<TRight, Task> fRight)
		where TEither : IEither<TEither, TLeft, TRight> =>
		await Match<TEither, TLeft, TRight, Task>(either,
			fLeft: x => { fLeft(x); return Task.CompletedTask; },
			fRight: fRight
		);

	/// <inheritdoc cref="Match{TEither, TLeft, TRight}(TEither, Action{TLeft}, Action{TRight})"/>
	public static async Task MatchAsync<TEither, TLeft, TRight>(TEither either, Func<TLeft, Task> fLeft, Action<TRight> fRight)
		where TEither : IEither<TEither, TLeft, TRight> =>
		await Match<TEither, TLeft, TRight, Task>(either,
			fLeft: fLeft,
			fRight: x => { fRight(x); return Task.CompletedTask; }
		);

	/// <inheritdoc cref="Match{TEither, TLeft, TRight}(TEither, Action{TLeft}, Action{TRight})"/>
	public static async Task MatchAsync<TEither, TLeft, TRight>(TEither either, Func<TLeft, Task> fLeft, Func<TRight, Task> fRight)
		where TEither : IEither<TEither, TLeft, TRight> =>
		await Match(either, fLeft, fRight);

	/// <inheritdoc cref="Match{TEither, TLeft, TRight}(TEither, Action{TLeft}, Action{TRight})"/>
	public static async Task MatchAsync<TEither, TLeft, TRight>(Task<TEither> either, Func<TLeft, Task> fLeft, Func<TRight, Task> fRight)
		where TEither : IEither<TEither, TLeft, TRight> =>
		await MatchAsync(await either, fLeft, fRight);

	#endregion

	#region With Return Value

	/// <summary>
	/// Run a function based on the value of <paramref name="either"/> and return its value.
	/// </summary>
	/// <remarks>
	/// Works with any <see cref="IEither{TEither, TLeft, TRight}"/>, for example <see cref="Either{TLeft, TRight}"/>,
	/// <c>Maybe&lt;T&gt;</c> or <c>Result&lt;T&gt;</c>, by calling <see cref="IEither{TLeft, TRight}.Match{TReturn}"/>.
	/// For an <see cref="Either{TLeft, TRight}"/> a <c>switch</c> over <see cref="Left{T}"/> and <see cref="Right{T}"/>
	/// does the same, and the compiler checks that both cases are handled.
	/// </remarks>
	/// <typeparam name="TEither">Either implementation type.</typeparam>
	/// <typeparam name="TLeft">Left (error / invalid) value type.</typeparam>
	/// <typeparam name="TRight">Right (correct / valid) value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="either">Either object.</param>
	/// <param name="fLeft">Function to run with the value when <paramref name="either"/> holds a left value.</param>
	/// <param name="fRight">Function to run with the value when <paramref name="either"/> holds a right value.</param>
	/// <returns>Value returned by <paramref name="fLeft"/> or <paramref name="fRight"/>.</returns>
	/// <exception cref="NullEitherException"><paramref name="either"/> is <see langword="null"/>.</exception>
	public static TReturn Match<TEither, TLeft, TRight, TReturn>(TEither either, Func<TLeft, TReturn> fLeft, Func<TRight, TReturn> fRight)
		where TEither : IEither<TEither, TLeft, TRight> =>
		(either ?? throw new NullEitherException()).Match(fLeft, fRight);

	/// <inheritdoc cref="Match{TEither, TLeft, TRight, TReturn}(TEither, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static async Task<TReturn> MatchAsync<TEither, TLeft, TRight, TReturn>(TEither either, Func<TLeft, TReturn> fLeft, Func<TRight, Task<TReturn>> fRight)
		where TEither : IEither<TEither, TLeft, TRight> =>
		await Match<TEither, TLeft, TRight, Task<TReturn>>(either,
			fLeft: x => Task.FromResult(fLeft(x)),
			fRight: fRight
		);

	/// <inheritdoc cref="Match{TEither, TLeft, TRight, TReturn}(TEither, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static async Task<TReturn> MatchAsync<TEither, TLeft, TRight, TReturn>(TEither either, Func<TLeft, Task<TReturn>> fLeft, Func<TRight, TReturn> fRight)
		where TEither : IEither<TEither, TLeft, TRight> =>
		await Match<TEither, TLeft, TRight, Task<TReturn>>(either,
			fLeft: fLeft,
			fRight: x => Task.FromResult(fRight(x))
		);

	/// <inheritdoc cref="Match{TEither, TLeft, TRight, TReturn}(TEither, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static async Task<TReturn> MatchAsync<TEither, TLeft, TRight, TReturn>(TEither either, Func<TLeft, Task<TReturn>> fLeft, Func<TRight, Task<TReturn>> fRight)
		where TEither : IEither<TEither, TLeft, TRight> =>
		await Match(either, fLeft, fRight);

	/// <inheritdoc cref="Match{TEither, TLeft, TRight, TReturn}(TEither, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static async Task<TReturn> MatchAsync<TEither, TLeft, TRight, TReturn>(Task<TEither> either, Func<TLeft, TReturn> fLeft, Func<TRight, TReturn> fRight)
		where TEither : IEither<TEither, TLeft, TRight> =>
		Match(await either, fLeft, fRight);

	/// <inheritdoc cref="Match{TEither, TLeft, TRight, TReturn}(TEither, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static async Task<TReturn> MatchAsync<TEither, TLeft, TRight, TReturn>(Task<TEither> either, Func<TLeft, TReturn> fLeft, Func<TRight, Task<TReturn>> fRight)
		where TEither : IEither<TEither, TLeft, TRight> =>
		await MatchAsync(await either, fLeft, fRight);

	/// <inheritdoc cref="Match{TEither, TLeft, TRight, TReturn}(TEither, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static async Task<TReturn> MatchAsync<TEither, TLeft, TRight, TReturn>(Task<TEither> either, Func<TLeft, Task<TReturn>> fLeft, Func<TRight, TReturn> fRight)
		where TEither : IEither<TEither, TLeft, TRight> =>
		await MatchAsync(await either, fLeft, fRight);

	/// <inheritdoc cref="Match{TEither, TLeft, TRight, TReturn}(TEither, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static async Task<TReturn> MatchAsync<TEither, TLeft, TRight, TReturn>(Task<TEither> either, Func<TLeft, Task<TReturn>> fLeft, Func<TRight, Task<TReturn>> fRight)
		where TEither : IEither<TEither, TLeft, TRight> =>
		await MatchAsync(await either, fLeft, fRight);

	#endregion
}
