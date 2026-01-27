// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Run <paramref name="f"/> is <paramref name="this"/> is <see cref="Some{T}"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="f">Function to run when <paramref name="this"/> is <see cref="Some{T}"/>.</param>
	/// <returns>The original value of <paramref name="this"/>.</returns>
	public static Maybe<T> IfSome<T>(this Maybe<T> @this, Action<T> f) =>
		@this.Audit(some: f);

	/// <inheritdoc cref="IfSome{T}(Maybe{T}, Action{T})"/>
	public static Task<Maybe<T>> IfSomeAsync<T>(this Maybe<T> @this, Func<T, Task> f) =>
		IfSomeAsync(@this.AsTask(), f);

	/// <inheritdoc cref="IfSome{T}(Maybe{T}, Action{T})"/>
	public static Task<Maybe<T>> IfSomeAsync<T>(this Task<Maybe<T>> @this, Action<T> f) =>
		IfSomeAsync(@this, x => { f(x); return Task.CompletedTask; });

	/// <inheritdoc cref="IfSome{T}(Maybe{T}, Action{T})"/>
	public static Task<Maybe<T>> IfSomeAsync<T>(this Task<Maybe<T>> @this, Func<T, Task> f) =>
		@this.AuditAsync(some: f);
}
