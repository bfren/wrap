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
	public static Result<T> IfFail<T>(this Result<T> @this, Action<FailValue> f) =>
		@this.Audit(fail: f);

	/// <inheritdoc cref="IfFail{T}(Result{T}, Action{FailValue})"/>
	public static Task<Result<T>> IfFailAsync<T>(this Result<T> @this, Func<FailValue, Task> f) =>
		IfFailAsync(@this.AsTask(), f);

	/// <inheritdoc cref="IfFail{T}(Result{T}, Action{FailValue})"/>
	public static Task<Result<T>> IfFailAsync<T>(this Task<Result<T>> @this, Action<FailValue> f) =>
		IfFailAsync(@this, x => { f(x); return Task.CompletedTask; });

	/// <inheritdoc cref="IfFail{T}(Result{T}, Action{FailValue})"/>
	public static Task<Result<T>> IfFailAsync<T>(this Task<Result<T>> @this, Func<FailValue, Task> f) =>
		@this.AuditAsync(fail: f);
}
