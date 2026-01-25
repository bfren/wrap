// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ResultExtensions
{
	/// <summary>
	/// Run <paramref name="f"/> is <paramref name="this"/> is <see cref="Ok{T}"/>.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="f">Function to run when <paramref name="this"/> is <see cref="Ok{T}"/>.</param>
	/// <returns>The original value of <paramref name="this"/>.</returns>
	public static Result<T> IfOk<T>(this Result<T> @this, Action<T> f) =>
		@this.Audit(ok: f);

	/// <inheritdoc cref="IfOk{T}(Result{T}, Action{T})"/>
	public static Task<Result<T>> IfOkAsync<T>(this Result<T> @this, Func<T, Task> f) =>
		IfOkAsync(@this.AsTask(), f);

	/// <inheritdoc cref="IfOk{T}(Result{T}, Action{T})"/>
	public static Task<Result<T>> IfOkAsync<T>(this Task<Result<T>> @this, Action<T> f) =>
		IfOkAsync(@this, x => { f(x); return Task.CompletedTask; });

	/// <inheritdoc cref="IfOk{T}(Result{T}, Action{T})"/>
	public static Task<Result<T>> IfOkAsync<T>(this Task<Result<T>> @this, Func<T, Task> f) =>
		@this.AuditAsync(ok: f);
}
