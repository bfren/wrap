// Wrap: .NET monads for functional style.
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
	public static Result<T> IfFailure<T>(this Result<T> @this, Action<FailureValue> f) =>
		Audit(@this, fail: f);

	/// <inheritdoc cref="IfFailure{T}(Result{T}, Action{FailureValue})"/>
	public static Task<Result<T>> IfFailureAsync<T>(this Result<T> @this, Func<FailureValue, Task> f) =>
		AuditAsync(@this, fail: f);

	/// <inheritdoc cref="IfFailure{T}(Result{T}, Action{FailureValue})"/>
	public static Task<Result<T>> IfFailureAsync<T>(this Task<Result<T>> @this, Action<FailureValue> f) =>
		AuditAsync(@this, fail: f);

	/// <inheritdoc cref="IfFailure{T}(Result{T}, Action{FailureValue})"/>
	public static Task<Result<T>> IfFailureAsync<T>(this Task<Result<T>> @this, Func<FailureValue, Task> f) =>
		AuditAsync(@this, fail: f);
}
