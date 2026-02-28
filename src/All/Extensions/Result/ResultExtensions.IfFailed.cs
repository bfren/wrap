// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ResultExtensions
{
	/// <summary>
	/// Run <paramref name="f"/> is <paramref name="this"/> is <see cref="Failure"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="f">Function to run when <paramref name="this"/> is <see cref="Failure"/>.</param>
	/// <returns>The original value of <paramref name="this"/>.</returns>
	public static Result<T> IfFailed<T>(this Result<T> @this, Action<FailureValue> f) =>
		Audit(@this, fFail: f);

	/// <inheritdoc cref="IfFailed{T}(Result{T}, Action{FailureValue})"/>
	public static Task<Result<T>> IfFailedAsync<T>(this Result<T> @this, Func<FailureValue, Task> f) =>
		AuditAsync(@this, fFail: f);

	/// <inheritdoc cref="IfFailed{T}(Result{T}, Action{FailureValue})"/>
	public static Task<Result<T>> IfFailedAsync<T>(this Task<Result<T>> @this, Action<FailureValue> f) =>
		AuditAsync(@this, fFail: f);

	/// <inheritdoc cref="IfFailed{T}(Result{T}, Action{FailureValue})"/>
	public static Task<Result<T>> IfFailedAsync<T>(this Task<Result<T>> @this, Func<FailureValue, Task> f) =>
		AuditAsync(@this, fFail: f);
}
