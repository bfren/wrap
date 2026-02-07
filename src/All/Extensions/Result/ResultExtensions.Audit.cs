// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ResultExtensions
{
	/// <inheritdoc cref="Audit{T}(Result{T}, Action{FailureValue}?, Action{T}?)"/>
	public static Result<T> Audit<T>(this Result<T> @this, Action<FailureValue> fFail) =>
		Audit(@this, fFail, null);

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{FailureValue}?, Action{T}?)"/>
	public static Result<T> Audit<T>(this Result<T> @this, Action<T> fOk) =>
		Audit(@this, null, fOk);

	/// <summary>
	/// Run <paramref name="fFail"/> or <paramref name="fOk"/> depending on the value of <paramref name="this"/>.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="fFail">Audit function to run when <paramref name="this"/> is <see cref="Failure"/>.</param>
	/// <param name="fOk">Audit function to run when <paramref name="this"/> is <see cref="Ok{T}"/>.</param>
	/// <returns>The original value of <paramref name="this"/>.</returns>
	public static Result<T> Audit<T>(this Result<T> @this, Action<FailureValue>? fFail, Action<T>? fOk)
	{
		try
		{
			if (@this is Result<T>.FailureImpl y && fFail is not null)
			{
				fFail(y.Value);
			}
			else if (@this is Ok<T> x && fOk is not null)
			{
				fOk(x.Value);
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
	public static Task<Result<T>> AuditAsync<T>(this Result<T> @this, Func<FailureValue, Task> fFail) =>
		AuditAsync(@this, fFail, null);

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{FailureValue}?, Action{T}?)"/>
	public static Task<Result<T>> AuditAsync<T>(this Result<T> @this, Func<T, Task> fOk) =>
		AuditAsync(@this, null, fOk);

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{FailureValue}?, Action{T}?)"/>
	public static Task<Result<T>> AuditAsync<T>(this Result<T> @this, Func<FailureValue, Task>? fFail, Func<T, Task>? fOk) =>
		AuditAsync(@this.AsTask(), fFail: fFail, fOk: fOk);

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{FailureValue}?, Action{T}?)"/>
	public static Task<Result<T>> AuditAsync<T>(this Task<Result<T>> @this, Action<FailureValue> fFail) =>
		AuditAsync(@this, fFail, null);

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{FailureValue}?, Action{T}?)"/>
	public static Task<Result<T>> AuditAsync<T>(this Task<Result<T>> @this, Action<T> fOk) =>
		AuditAsync(@this, null, fOk);

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{FailureValue}?, Action{T}?)"/>
	public static Task<Result<T>> AuditAsync<T>(this Task<Result<T>> @this, Action<FailureValue>? fFail, Action<T>? fOk) =>
		AuditAsync(@this, fFail: async x => fFail?.Invoke(x), fOk: async x => fOk?.Invoke(x));

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{FailureValue}?, Action{T}?)"/>
	public static Task<Result<T>> AuditAsync<T>(this Task<Result<T>> @this, Func<FailureValue, Task> fFail) =>
		AuditAsync(@this, fFail, null);

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{FailureValue}?, Action{T}?)"/>
	public static Task<Result<T>> AuditAsync<T>(this Task<Result<T>> @this, Func<T, Task> fOk) =>
		AuditAsync(@this, null, fOk);

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{FailureValue}?, Action{T}?)"/>
	public static async Task<Result<T>> AuditAsync<T>(this Task<Result<T>> @this, Func<FailureValue, Task>? fFail, Func<T, Task>? fOk)
	{
		var result = await @this;

		try
		{
			if (result is Result<T>.FailureImpl y && fFail is not null)
			{
				await fFail(y.Value);
			}
			else if (result is Ok<T> x && fOk is not null)
			{
				await fOk(x.Value);
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
		AuditAsync(@this.AsTask(), either: either);

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{Result{T}})"/>
	public static Task<Result<T>> AuditAsync<T>(this Task<Result<T>> @this, Action<Result<T>> either) =>
		AuditAsync(@this, either: async x => either(x));

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
