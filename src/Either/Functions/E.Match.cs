// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap;

public static partial class E
{
	#region Without Return Value

	/// <summary>
	/// Run an action based on the value of <paramref name="either"/>.
	/// </summary>
	/// <remarks>
	/// This is shorthand for a <c>switch</c> over the <see cref="Left{T}"/> and <see cref="Right{T}"/> cases
	/// that passes the unwrapped value to <paramref name="fLeft"/> or <paramref name="fRight"/>. Both approaches
	/// make you handle both cases: here through the required arguments, and in a <c>switch</c> through the
	/// compiler's exhaustiveness check.
	/// </remarks>
	/// <typeparam name="TLeft">Left (error / invalid) value type.</typeparam>
	/// <typeparam name="TRight">Right (correct / valid) value type.</typeparam>
	/// <param name="either">Either object.</param>
	/// <param name="fLeft">Action to run with the value when <paramref name="either"/> is a <see cref="Left{T}"/>.</param>
	/// <param name="fRight">Action to run with the value when <paramref name="either"/> is a <see cref="Right{T}"/>.</param>
	public static void Match<TLeft, TRight>(Either<TLeft, TRight> either, Action<TLeft> fLeft, Action<TRight> fRight)
	{
		switch (either)
		{
			case Left<TLeft> l:
				fLeft(l.Value);
				return;

			case Right<TRight> r:
				fRight(r.Value);
				return;

			default:
				break;
		}
	}

	/// <inheritdoc cref="Match{TLeft, TRight}(Either{TLeft, TRight}, Action{TLeft}, Action{TRight})"/>
	public static async Task MatchAsync<TLeft, TRight>(Either<TLeft, TRight> either, Action<TLeft> fLeft, Func<TRight, Task> fRight)
	{
		switch (either)
		{
			case Left<TLeft> l:
				fLeft(l.Value);
				return;

			case Right<TRight> r:
				await fRight(r.Value);
				return;

			default:
				break;
		}
	}

	/// <inheritdoc cref="Match{TLeft, TRight}(Either{TLeft, TRight}, Action{TLeft}, Action{TRight})"/>
	public static async Task MatchAsync<TLeft, TRight>(Either<TLeft, TRight> either, Func<TLeft, Task> fLeft, Action<TRight> fRight)
	{
		switch (either)
		{
			case Left<TLeft> l:
				await fLeft(l.Value);
				return;

			case Right<TRight> r:
				fRight(r.Value);
				return;

			default:
				break;
		}
	}

	/// <inheritdoc cref="Match{TLeft, TRight}(Either{TLeft, TRight}, Action{TLeft}, Action{TRight})"/>
	public static async Task MatchAsync<TLeft, TRight>(Either<TLeft, TRight> either, Func<TLeft, Task> fLeft, Func<TRight, Task> fRight)
	{
		switch (either)
		{
			case Left<TLeft> l:
				await fLeft(l.Value);
				return;

			case Right<TRight> r:
				await fRight(r.Value);
				return;

			default:
				break;
		}
	}

	/// <inheritdoc cref="Match{TLeft, TRight}(Either{TLeft, TRight}, Action{TLeft}, Action{TRight})"/>
	public static async Task MatchAsync<TLeft, TRight>(Task<Either<TLeft, TRight>> either, Func<TLeft, Task> fLeft, Func<TRight, Task> fRight) =>
		await MatchAsync(await either, fLeft, fRight);

	#endregion

	#region With Return Value

	/// <summary>
	/// Run a function based on the value of <paramref name="either"/> and return its value.
	/// </summary>
	/// <remarks>
	/// This is shorthand for a <c>switch</c> over the <see cref="Left{T}"/> and <see cref="Right{T}"/> cases
	/// that passes the unwrapped value to <paramref name="fLeft"/> or <paramref name="fRight"/>. Both approaches
	/// make you handle both cases: here through the required arguments, and in a <c>switch</c> through the
	/// compiler's exhaustiveness check.
	/// </remarks>
	/// <typeparam name="TLeft">Left (error / invalid) value type.</typeparam>
	/// <typeparam name="TRight">Right (correct / valid) value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="either">Either object.</param>
	/// <param name="fLeft">Function to run with the value when <paramref name="either"/> is a <see cref="Left{T}"/>.</param>
	/// <param name="fRight">Function to run with the value when <paramref name="either"/> is a <see cref="Right{T}"/>.</param>
	/// <returns>Value returned by <paramref name="fLeft"/> or <paramref name="fRight"/>.</returns>
	public static TReturn Match<TLeft, TRight, TReturn>(Either<TLeft, TRight> either, Func<TLeft, TReturn> fLeft, Func<TRight, TReturn> fRight) =>
		either switch
		{
			Left<TLeft> l =>
				fLeft(l.Value),

			Right<TRight> r =>
				fRight(r.Value)
		};

	/// <inheritdoc cref="Match{TLeft, TRight, TReturn}(Either{TLeft, TRight}, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static async Task<TReturn> MatchAsync<TLeft, TRight, TReturn>(Either<TLeft, TRight> either, Func<TLeft, TReturn> fLeft, Func<TRight, Task<TReturn>> fRight) =>
		either switch
		{
			Left<TLeft> l =>
				fLeft(l.Value),

			Right<TRight> r =>
				await fRight(r.Value)
		};

	/// <inheritdoc cref="Match{TLeft, TRight, TReturn}(Either{TLeft, TRight}, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static async Task<TReturn> MatchAsync<TLeft, TRight, TReturn>(Either<TLeft, TRight> either, Func<TLeft, Task<TReturn>> fLeft, Func<TRight, TReturn> fRight) =>
		either switch
		{
			Left<TLeft> l =>
				await fLeft(l.Value),

			Right<TRight> r =>
				fRight(r.Value)
		};

	/// <inheritdoc cref="Match{TLeft, TRight, TReturn}(Either{TLeft, TRight}, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static async Task<TReturn> MatchAsync<TLeft, TRight, TReturn>(Either<TLeft, TRight> either, Func<TLeft, Task<TReturn>> fLeft, Func<TRight, Task<TReturn>> fRight) =>
		either switch
		{
			Left<TLeft> l =>
				await fLeft(l.Value),

			Right<TRight> r =>
				await fRight(r.Value)
		};

	/// <inheritdoc cref="Match{TLeft, TRight, TReturn}(Either{TLeft, TRight}, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static async Task<TReturn> MatchAsync<TLeft, TRight, TReturn>(Task<Either<TLeft, TRight>> either, Func<TLeft, TReturn> fLeft, Func<TRight, TReturn> fRight) =>
		Match(await either, fLeft, fRight);

	/// <inheritdoc cref="Match{TLeft, TRight, TReturn}(Either{TLeft, TRight}, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static async Task<TReturn> MatchAsync<TLeft, TRight, TReturn>(Task<Either<TLeft, TRight>> either, Func<TLeft, TReturn> fLeft, Func<TRight, Task<TReturn>> fRight) =>
		await MatchAsync(await either, fLeft, fRight);

	/// <inheritdoc cref="Match{TLeft, TRight, TReturn}(Either{TLeft, TRight}, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static async Task<TReturn> MatchAsync<TLeft, TRight, TReturn>(Task<Either<TLeft, TRight>> either, Func<TLeft, Task<TReturn>> fLeft, Func<TRight, TReturn> fRight) =>
		await MatchAsync(await either, fLeft, fRight);

	/// <inheritdoc cref="Match{TLeft, TRight, TReturn}(Either{TLeft, TRight}, Func{TLeft, TReturn}, Func{TRight, TReturn})"/>
	public static async Task<TReturn> MatchAsync<TLeft, TRight, TReturn>(Task<Either<TLeft, TRight>> either, Func<TLeft, Task<TReturn>> fLeft, Func<TRight, Task<TReturn>> fRight) =>
		await MatchAsync(await either, fLeft, fRight);

	#endregion
}
