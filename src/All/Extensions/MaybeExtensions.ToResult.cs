// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class MaybeExtensions
{
	private const string NoneFailureMessage = "Maybe<{Type}> was 'None'.";

	/// <summary>
	/// Convert a <see cref="Maybe{T}"/> to a <see cref="Result{T}"/>, converting <see cref="None"/>
	/// values to a <see cref="Failure"/>.
	/// </summary>
	/// <typeparam name="T">Value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="class">Context class.</param>
	/// <param name="function">Context function.</param>
	/// <returns>Result object.</returns>
	public static Result<T> ToResult<T>(this Maybe<T> @this, string @class, string function) =>
		ToResult(@this,
			() => R.Fail(NoneFailureMessage, typeof(T).Name)
				.Ctx(@class, function)
		);

	/// <inheritdoc cref="ToResult{T}(Maybe{T}, string, string)"/>
	public static Task<Result<T>> ToResultAsync<T>(this Task<Maybe<T>> @this, string @class, string function) =>
		ToResultAsync(@this,
			() => R.Fail(NoneFailureMessage, typeof(T).Name)
				.Ctx(@class, function)
		);

	/// <summary>
	/// Convert a <see cref="Maybe{T}"/> to a <see cref="Result{T}"/>.
	/// </summary>
	/// <typeparam name="T">Value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="noneHandler">What to do when <paramref name="this"/> is <see cref="None"/>.</param>
	/// <returns>Result object.</returns>
	public static Result<T> ToResult<T>(this Maybe<T> @this, Func<Result<T>> noneHandler) =>
		@this.Match(
			none: () => noneHandler(),
			some: R.Wrap
		);

	/// <inheritdoc cref="ToResult{T}(Maybe{T}, Func{Result{T}})"/>
	public static Task<Result<T>> ToResultAsync<T>(this Task<Maybe<T>> @this, Func<Result<T>> noneHandler) =>
		ToResultAsync(@this,
			() => noneHandler().AsTask()
		);

	/// <inheritdoc cref="ToResult{T}(Maybe{T}, Func{Result{T}})"/>
	public static Task<Result<T>> ToResultAsync<T>(this Task<Maybe<T>> @this, Func<Task<Result<T>>> noneHandler) =>
		@this.MatchAsync(
			none: () => noneHandler(),
			some: R.Wrap
		);
}
