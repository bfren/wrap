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
	/// <param name="fFailureHandler">Do something with the FailureValue before discarding it.</param>
	/// <returns>Maybe object.</returns>
	public static Maybe<T> ToMaybe<T>(this Result<T> @this, Action<FailureValue> fFailureHandler) =>
		@this.Match(fFail: f => { fFailureHandler(f); return M.None; }, fOk: M.Wrap);

	/// <inheritdoc cref="ToMaybe{T}(Result{T}, Action{FailureValue})"/>
	public static Task<Maybe<T>> ToMaybeAsync<T>(this Task<Result<T>> @this, Action<FailureValue> fFailureHandler) =>
		ToMaybeAsync(@this, async f => { fFailureHandler(f); return M.None; });

	/// <inheritdoc cref="ToMaybe{T}(Result{T}, Action{FailureValue})"/>
	public static Task<Maybe<T>> ToMaybeAsync<T>(this Task<Result<T>> @this, Func<FailureValue, Maybe<T>> fFailureHandler) =>
		ToMaybeAsync(@this, async f => fFailureHandler(f));

	/// <inheritdoc cref="ToMaybe{T}(Result{T}, Action{FailureValue})"/>
	public static Task<Maybe<T>> ToMaybeAsync<T>(this Task<Result<T>> @this, Func<FailureValue, Task> fFailureHandler) =>
		ToMaybeAsync(@this, async f => { await fFailureHandler(f); return M.None; });

	/// <inheritdoc cref="ToMaybe{T}(Result{T}, Action{FailureValue})"/>
	public static Task<Maybe<T>> ToMaybeAsync<T>(this Task<Result<T>> @this, Func<FailureValue, Task<Maybe<T>>> fFailureHandler) =>
		@this.MatchAsync(fFail: f => fFailureHandler(f), fOk: M.Wrap);
}
