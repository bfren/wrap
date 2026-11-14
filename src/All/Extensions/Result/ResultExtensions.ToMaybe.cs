// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ResultExtensions
{
	/// <summary>
	/// Convert a <see cref="Result{T}"/> to a <see cref="Maybe{T}"/>, passing any <see cref="Failure"/> to <paramref name="fFailureHandler"/>.
	/// </summary>
	/// <remarks>
	/// WARNING: this discards the <see cref="Failure"/> that explains why an operation failed.
	/// Only use it if <paramref name="fFailureHandler"/> logs the failure, or you *really* don't care.
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="fFailureHandler">Run with the <see cref="Failure"/> before it is discarded, for example to log it.</param>
	/// <returns><see cref="Some{T}"/> holding the value, or <see cref="None"/> if <paramref name="this"/> is a <see cref="Failure"/>.</returns>
	public static Maybe<T> ToMaybe<T>(this Result<T> @this, Action<Failure> fFailureHandler)
	{
		switch (@this)
		{
			case Ok<T> x:
				return x.Value;

			// default, not `case Failure x:`: a Failure arm makes the JIT set up a 40-byte Failure local on every call,
			// even on the Ok path (see R.Match)
			default:
				fFailureHandler(@this.GetFailure());
				return M.None;
		}
	}

	/// <inheritdoc cref="ToMaybe{T}(Result{T}, Action{Failure})"/>
	public static async Task<Maybe<T>> ToMaybeAsync<T>(this Task<Result<T>> @this, Action<Failure> fFailureHandler)
	{
		switch (await @this)
		{
			case Ok<T> x:
				return x.Value;

			case Failure x:
				fFailureHandler(x);
				return M.None;

			// IDE0010 does not treat the union cases as complete, so it needs this arm
			default:
				return M.None;
		}
	}

	/// <summary>
	/// Convert a <see cref="Result{T}"/> to a <see cref="Maybe{T}"/>, using <paramref name="fFailureHandler"/> when it is a <see cref="Failure"/>.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="fFailureHandler">Returns the <see cref="Maybe{T}"/> to use when <paramref name="this"/> is a <see cref="Failure"/>.</param>
	/// <returns><see cref="Some{T}"/> holding the value, or the <see cref="Maybe{T}"/> returned by <paramref name="fFailureHandler"/>.</returns>
	public static async Task<Maybe<T>> ToMaybeAsync<T>(this Task<Result<T>> @this, Func<Failure, Maybe<T>> fFailureHandler) =>
		await @this switch
		{
			Ok<T> x =>
				x.Value,

			Failure x =>
				fFailureHandler(x)
		};

	/// <inheritdoc cref="ToMaybe{T}(Result{T}, Action{Failure})"/>
	public static async Task<Maybe<T>> ToMaybeAsync<T>(this Task<Result<T>> @this, Func<Failure, Task> fFailureHandler)
	{
		switch (await @this)
		{
			case Ok<T> x:
				return x.Value;

			case Failure x:
				await fFailureHandler(x);
				return M.None;

			// IDE0010 does not treat the union cases as complete, so it needs this arm
			default:
				return M.None;
		}
	}

	/// <inheritdoc cref="ToMaybeAsync{T}(Task{Result{T}}, Func{Failure, Maybe{T}})"/>
	public static async Task<Maybe<T>> ToMaybeAsync<T>(this Task<Result<T>> @this, Func<Failure, Task<Maybe<T>>> fFailureHandler) =>
		await @this switch
		{
			Ok<T> x =>
				x.Value,

			Failure x =>
				await fFailureHandler(x)
		};
}
