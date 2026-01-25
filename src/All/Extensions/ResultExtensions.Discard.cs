// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ResultExtensions
{
	/// <summary>
	/// Discard the failure value of a <see cref="Result{T}"/> and return a <see cref="Maybe{T}"/>.
	/// </summary>
	/// <remarks>
	/// WARNING: this will discard the <see cref="Fail"/> value that explains why an operation failed.
	/// You should only use this method if you have logged the failure value, or *really* don't care.
	/// </remarks>
	/// <typeparam name="T">Value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <returns>Maybe object.</returns>
	public static Maybe<T> Discard<T>(this Result<T> @this) =>
		@this.Match(
			fail: _ => M.None,
			ok: x => M.Wrap(x)
		);

	/// <inheritdoc cref="Discard{T}(Result{T})"/>
	public static Task<Maybe<T>> DiscardAsync<T>(this Task<Result<T>> @this) =>
		@this.MatchAsync(
			fail: _ => M.None,
			ok: x => M.Wrap(x)
		);
}
