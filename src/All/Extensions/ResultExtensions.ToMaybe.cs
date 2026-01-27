// Wrap: .NET monads for functional style.
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
	/// <param name="failureHandler">Do something with the FailureValue before discarding it.</param>
	/// <returns>Maybe object.</returns>
	public static Maybe<T> ToMaybe<T>(this Result<T> @this, Action<FailureValue> failureHandler) =>
		@this.Match(
			fail: f => { failureHandler(f); return M.None; },
			ok: M.Wrap
		);

	/// <inheritdoc cref="ToMaybe{T}(Result{T}, Action{FailureValue})"/>
	public static Task<Maybe<T>> ToMaybeAsync<T>(this Task<Result<T>> @this, Action<FailureValue> failureHandler) =>
		ToMaybeAsync(@this, f => { failureHandler(f); return M.NoneAsTask<T>(); });

	/// <inheritdoc cref="ToMaybe{T}(Result{T}, Action{FailureValue})"/>
	public static Task<Maybe<T>> ToMaybeAsync<T>(this Task<Result<T>> @this, Func<FailureValue, Maybe<T>> failureHandler) =>
		ToMaybeAsync(@this, f => failureHandler(f).AsTask());

	/// <inheritdoc cref="ToMaybe{T}(Result{T}, Action{FailureValue})"/>
	public static Task<Maybe<T>> ToMaybeAsync<T>(this Task<Result<T>> @this, Func<FailureValue, Task> failureHandler) =>
		ToMaybeAsync(@this, async f => { await failureHandler(f); return M.None; });

	/// <inheritdoc cref="ToMaybe{T}(Result{T}, Action{FailureValue})"/>
	public static Task<Maybe<T>> ToMaybeAsync<T>(this Task<Result<T>> @this, Func<FailureValue, Task<Maybe<T>>> failureHandler) =>
		@this.MatchAsync(
			fail: f => failureHandler(f),
			ok: M.Wrap
		);
}
