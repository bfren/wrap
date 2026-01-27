// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ResultExtensions
{
	/// <inheritdoc cref="Audit{T}(Result{T}, Action{FailureValue}?, Action{T}?)"/>
	public static Result<T> Audit<T>(this Result<T> @this, Action<FailureValue> fail) =>
		Audit(@this, fail, null);

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{FailureValue}?, Action{T}?)"/>
	public static Result<T> Audit<T>(this Result<T> @this, Action<T> ok) =>
		Audit(@this, null, ok);

	/// <summary>
	/// Run <paramref name="fail"/> or <paramref name="ok"/> depending on the value of <paramref name="this"/>.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="fail">Audit function to run when <paramref name="this"/> is <see cref="Failure"/>.</param>
	/// <param name="ok">Audit function to run when <paramref name="this"/> is <see cref="Ok{T}"/>.</param>
	/// <returns>The original value of <paramref name="this"/>.</returns>
	public static Result<T> Audit<T>(this Result<T> @this, Action<FailureValue>? fail, Action<T>? ok)
	{
		try
		{
			if (@this is Result<T>.Failure y && fail is not null)
			{
				fail(y.Value);
			}
			else if (@this is Ok<T> x && ok is not null)
			{
				ok(x.Value);
			}
		}
		catch (Exception ex)
		{
			F.LogException?.Invoke(ex);
		}

		return @this;
	}

	/// <summary>
	/// Run <paramref name="either"/> with the value of <paramref name="this"/>.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="either">Audit function to run.</param>
	/// <returns>The original value of <paramref name="this"/>.</returns>
	public static Result<T> Audit<T>(this Result<T> @this, Action<Result<T>> either)
	{
		try
		{
			either(@this);
		}
		catch (Exception ex)
		{
			F.LogException?.Invoke(ex);
		}

		return @this;
	}

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{FailureValue}?, Action{T}?)"/>
	public static Task<Result<T>> AuditAsync<T>(this Result<T> @this, Func<FailureValue, Task> fail) =>
		AuditAsync(@this, fail, null);

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{FailureValue}?, Action{T}?)"/>
	public static Task<Result<T>> AuditAsync<T>(this Result<T> @this, Func<T, Task> ok) =>
		AuditAsync(@this, null, ok);

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{FailureValue}?, Action{T}?)"/>
	public static Task<Result<T>> AuditAsync<T>(this Result<T> @this, Func<FailureValue, Task>? fail, Func<T, Task>? ok) =>
		AuditAsync(@this.AsTask(),
			fail: fail,
			ok: ok
		);

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{FailureValue}?, Action{T}?)"/>
	public static Task<Result<T>> AuditAsync<T>(this Task<Result<T>> @this, Action<FailureValue> fail) =>
		AuditAsync(@this, fail, null);

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{FailureValue}?, Action{T}?)"/>
	public static Task<Result<T>> AuditAsync<T>(this Task<Result<T>> @this, Action<T> ok) =>
		AuditAsync(@this, null, ok);

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{FailureValue}?, Action{T}?)"/>
	public static Task<Result<T>> AuditAsync<T>(this Task<Result<T>> @this, Action<FailureValue>? fail, Action<T>? ok) =>
		AuditAsync(@this,
			fail: x => { fail?.Invoke(x); return Task.CompletedTask; },
			ok: x => { ok?.Invoke(x); return Task.CompletedTask; }
		);

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{FailureValue}?, Action{T}?)"/>
	public static Task<Result<T>> AuditAsync<T>(this Task<Result<T>> @this, Func<FailureValue, Task> fail) =>
		AuditAsync(@this, fail, null);

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{FailureValue}?, Action{T}?)"/>
	public static Task<Result<T>> AuditAsync<T>(this Task<Result<T>> @this, Func<T, Task> ok) =>
		AuditAsync(@this, null, ok);

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{FailureValue}?, Action{T}?)"/>
	public static async Task<Result<T>> AuditAsync<T>(this Task<Result<T>> @this, Func<FailureValue, Task>? fail, Func<T, Task>? ok)
	{
		var result = await @this;

		try
		{
			if (result is Result<T>.Failure y && fail is not null)
			{
				await fail(y.Value);
			}
			else if (result is Ok<T> x && ok is not null)
			{
				await ok(x.Value);
			}
		}
		catch (Exception ex)
		{
			F.LogException?.Invoke(ex);
		}

		return result;
	}

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{Result{T}})"/>
	public static Task<Result<T>> AuditAsync<T>(this Result<T> @this, Func<Result<T>, Task> either) =>
		AuditAsync(@this.AsTask(),
			either: either
		);

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{Result{T}})"/>
	public static Task<Result<T>> AuditAsync<T>(this Task<Result<T>> @this, Action<Result<T>> either) =>
		AuditAsync(@this,
			either: x => { either(x); return Task.CompletedTask; }
		);

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{Result{T}})"/>
	public static async Task<Result<T>> AuditAsync<T>(this Task<Result<T>> @this, Func<Result<T>, Task> either)
	{
		var result = await @this;

		try
		{
			await either(result);
		}
		catch (Exception ex)
		{
			F.LogException?.Invoke(ex);
		}

		return result;
	}
}
