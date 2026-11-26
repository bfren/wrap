// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ResultExtensions
{
	#region Discarding the Failure

	/// <summary>
	/// Convert a <see cref="Result{T}"/> to a <see cref="Maybe{T}"/>, passing any <see cref="Failure"/> to <paramref name="fFail"/>.
	/// </summary>
	/// <remarks>
	/// WARNING: this discards the <see cref="Failure"/> that explains why an operation failed.
	/// Only use it if <paramref name="fFail"/> logs the failure, or you *really* don't care.
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="fFail">Run with the <see cref="Failure"/> before it is discarded, for example to log it.</param>
	/// <returns><see cref="Some{T}"/> holding the value, or <see cref="None"/> if <paramref name="this"/> is a <see cref="Failure"/>.</returns>
	public static Maybe<T> ToMaybe<T>(this Result<T> @this, Action<Failure> fFail)
	{
		switch (@this)
		{
			case Ok<T> x:
				return x.Value;

			// default, not `case Failure x:`: a Failure arm makes the JIT set up a 40-byte Failure local on every call,
			// even on the Ok path (see R.Match)
			default:
				fFail(@this.GetFailure());
				return M.None;
		}
	}

	/// <inheritdoc cref="ToMaybe{T}(Result{T}, Action{Failure})"/>
	public static async Task<Maybe<T>> ToMaybeAsync<T>(this Result<T> @this, Func<Failure, Task> fFail)
	{
		switch (@this)
		{
			case Ok<T> x:
				return x.Value;

			case Failure x:
				await fFail(x);
				return M.None;

			// IDE0010 does not treat the union cases as complete, so it needs this arm
			default:
				return M.None;
		}
	}

	/// <inheritdoc cref="ToMaybe{T}(Result{T}, Action{Failure})"/>
	public static async Task<Maybe<T>> ToMaybeAsync<T>(this Task<Result<T>> @this, Action<Failure> fFail) =>
		ToMaybe(await @this, fFail);

	/// <inheritdoc cref="ToMaybe{T}(Result{T}, Action{Failure})"/>
	public static async Task<Maybe<T>> ToMaybeAsync<T>(this Task<Result<T>> @this, Func<Failure, Task> fFail) =>
		await ToMaybeAsync(await @this, fFail);

	#endregion

	#region Replacing the Failure

	// there is no sync ToMaybe(Func<Failure, Maybe<T>>): a T converts to Maybe<T>, so a logging lambda that returns a T
	// (such as f => Log(f) on a Result<string>) would pick it over ToMaybe(Action<Failure>) and return Some instead of None

	/// <summary>
	/// Convert a <see cref="Result{T}"/> to a <see cref="Maybe{T}"/>, using <paramref name="fFail"/> when it is a <see cref="Failure"/>.
	/// </summary>
	/// <remarks>
	/// A <typeparamref name="T"/> converts to <see cref="Maybe{T}"/>, so a lambda that returns a <typeparamref name="T"/>
	/// picks this overload over the one that takes <see cref="Func{Failure, Task}"/>, and its value becomes the result.
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="fFail">Returns the <see cref="Maybe{T}"/> to use when <paramref name="this"/> is a <see cref="Failure"/>.</param>
	/// <returns><see cref="Some{T}"/> holding the value, or the <see cref="Maybe{T}"/> returned by <paramref name="fFail"/>.</returns>
	public static async Task<Maybe<T>> ToMaybeAsync<T>(this Result<T> @this, Func<Failure, Task<Maybe<T>>> fFail) =>
		@this switch
		{
			Ok<T> x =>
				x.Value,

			Failure x =>
				await fFail(x)
		};

	/// <inheritdoc cref="ToMaybeAsync{T}(Result{T}, Func{Failure, Task{Maybe{T}}})"/>
	public static async Task<Maybe<T>> ToMaybeAsync<T>(this Task<Result<T>> @this, Func<Failure, Maybe<T>> fFail) =>
		await @this switch
		{
			Ok<T> x =>
				x.Value,

			Failure x =>
				fFail(x)
		};

	/// <inheritdoc cref="ToMaybeAsync{T}(Result{T}, Func{Failure, Task{Maybe{T}}})"/>
	public static async Task<Maybe<T>> ToMaybeAsync<T>(this Task<Result<T>> @this, Func<Failure, Task<Maybe<T>>> fFail) =>
		await ToMaybeAsync(await @this, fFail);

	#endregion
}
