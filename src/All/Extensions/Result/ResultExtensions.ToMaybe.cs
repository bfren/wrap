// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ResultExtensions
{
	/// <summary>
	/// Discard the failure value of a <see cref="Result{T}"/> and return a <see cref="Maybe{T}"/>.
	/// </summary>
	/// <remarks>
	/// WARNING: this will discard the <see cref="Failure"/> value that explains why an operation failed.
	/// You should only use this method if you have logged the failure value, or *really* don't care.
	/// </remarks>
	/// <typeparam name="T">Value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="fFailureHandler">Do something with the Failure before discarding it.</param>
	/// <returns>Maybe object.</returns>
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

	/// <inheritdoc cref="ToMaybe{T}(Result{T}, Action{Failure})"/>
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

	/// <inheritdoc cref="ToMaybe{T}(Result{T}, Action{Failure})"/>
	public static async Task<Maybe<T>> ToMaybeAsync<T>(this Task<Result<T>> @this, Func<Failure, Task<Maybe<T>>> fFailureHandler) =>
		await @this switch
		{
			Ok<T> x =>
				x.Value,

			Failure x =>
				await fFailureHandler(x)
		};
}
