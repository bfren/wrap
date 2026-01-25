// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions;

public static partial class MaybeExtensions
{
	private const string NoneFailureMessage = "Maybe<{Type}> was 'None'.";

	/// <summary>
	/// Convert a <see cref="Maybe{T}"/> to a <see cref="Result{T}"/>.
	/// </summary>
	/// <typeparam name="T">Value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="class">Context class.</param>
	/// <param name="function">Context function.</param>
	/// <returns>Result object.</returns>
	public static Result<T> ToResult<T>(this Maybe<T> @this, string @class, string function) =>
		ToResult(@this, () => R.Fail(NoneFailureMessage, new { Type = typeof(T).FullName }).Ctx(@class, function));

	/// <summary>
	/// Convert a <see cref="Maybe{T}"/> to a <see cref="Result{T}"/>.
	/// </summary>
	/// <typeparam name="T">Value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="ifNone">Error handler when <paramref name="this"/> is <see cref="None"/>.</param>
	/// <returns>Result object.</returns>
	public static Result<T> ToResult<T>(this Maybe<T> @this, R.ErrorHandler ifNone) =>
		@this.Match(
			none: () => ifNone(),
			some: x => R.Wrap(x)
		);
}
