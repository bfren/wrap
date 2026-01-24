// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Run <paramref name="f"/> is <paramref name="this"/> is <see cref="None"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="f">Function to run when <paramref name="this"/> is <see cref="None"/>.</param>
	/// <returns>The original value of <paramref name="this"/>.</returns>
	public static Maybe<T> IfNone<T>(this Maybe<T> @this, Action f) =>
		@this.Audit(none: f);

	/// <inheritdoc cref="IfNone{T}(Maybe{T}, Action)"/>
	public static Task<Maybe<T>> IfNoneAsync<T>(this Maybe<T> @this, Func<Task> f) =>
		IfNoneAsync(@this.AsTask(), f);

	/// <inheritdoc cref="IfNone{T}(Maybe{T}, Action)"/>
	public static Task<Maybe<T>> IfNoneAsync<T>(this Task<Maybe<T>> @this, Action f) =>
		IfNoneAsync(@this, () => { f(); return Task.CompletedTask; });

	/// <inheritdoc cref="IfNone{T}(Maybe{T}, Action)"/>
	public static Task<Maybe<T>> IfNoneAsync<T>(this Task<Maybe<T>> @this, Func<Task> f) =>
		@this.AuditAsync(none: f);
}
