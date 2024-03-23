// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap;

public static partial class ResultExtensions
{
	/// <summary>
	/// Run <paramref name="f"/> is <paramref name="this"/> is <see cref="Fail"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="f">Function to run when <paramref name="this"/> is <see cref="Fail"/>.</param>
	/// <returns>The original value of <paramref name="this"/>.</returns>
	public static Result<T> IfErr<T>(this Result<T> @this, Action<FailValue> f) =>
		@this.Audit(err: f);

	/// <inheritdoc cref="IfErr{T}(Result{T}, Action{FailValue})"/>
	public static Task<Result<T>> IfErrAsync<T>(this Result<T> @this, Func<FailValue, Task> f) =>
		IfErrAsync(@this.AsTask(), f);

	/// <inheritdoc cref="IfErr{T}(Result{T}, Action{FailValue})"/>
	public static Task<Result<T>> IfErrAsync<T>(this Task<Result<T>> @this, Action<FailValue> f) =>
		IfErrAsync(@this, x => { f(x); return Task.CompletedTask; });

	/// <inheritdoc cref="IfErr{T}(Result{T}, Action{FailValue})"/>
	public static Task<Result<T>> IfErrAsync<T>(this Task<Result<T>> @this, Func<FailValue, Task> f) =>
		@this.AuditAsync(err: f);
}
