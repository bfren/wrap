// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap;

public static partial class ResultExtensions
{
	/// <summary>
	/// Run <paramref name="err"/> or <paramref name="ok"/> depending on the value of <paramref name="this"/>.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="err">Audit function to run when <paramref name="this"/> is <see cref="Fail"/>.</param>
	/// <param name="ok">Audit function to run when <paramref name="this"/> is <see cref="Ok{T}"/>.</param>
	/// <returns>The original value of <paramref name="this"/>.</returns>
	public static Result<T> Audit<T>(this Result<T> @this, Action<FailValue>? err = null, Action<T>? ok = null)
	{
		try
		{
			if (@this is Result<T>.Err y && err is not null)
			{
				err(y.Value);
			}
			else if (@this is Ok<T> x && ok is not null)
			{
				ok(x.Value);
			}
		}
		catch (Exception ex)
		{
			F.LogException(ex);
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
			F.LogException(ex);
		}

		return @this;
	}

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{FailValue}?, Action{T}?)"/>
	public static Task<Result<T>> AuditAsync<T>(this Result<T> @this, Func<FailValue, Task>? err = null, Func<T, Task>? ok = null) =>
		AuditAsync(@this.AsTask(), err, ok);

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{FailValue}?, Action{T}?)"/>
	public static Task<Result<T>> AuditAsync<T>(this Task<Result<T>> @this, Action<FailValue>? err = null, Action<T>? ok = null) =>
		AuditAsync(@this, x => { err?.Invoke(x); return Task.CompletedTask; }, x => { ok?.Invoke(x); return Task.CompletedTask; });

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{FailValue}?, Action{T}?)"/>
	public static async Task<Result<T>> AuditAsync<T>(this Task<Result<T>> @this, Func<FailValue, Task>? err = null, Func<T, Task>? ok = null)
	{
		var result = await @this;

		try
		{
			if (result is Result<T>.Err y && err is not null)
			{
				await err(y.Value);
			}
			else if (result is Ok<T> x && ok is not null)
			{
				await ok(x.Value);
			}
		}
		catch (Exception ex)
		{
			F.LogException(ex);
		}

		return result;
	}

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{Result{T}})"/>
	public static Task<Result<T>> AuditAsync<T>(this Result<T> @this, Func<Result<T>, Task> either) =>
		AuditAsync(@this.AsTask(), either);

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{Result{T}})"/>
	public static Task<Result<T>> AuditAsync<T>(this Task<Result<T>> @this, Action<Result<T>> either) =>
		AuditAsync(@this, x => { either(x); return Task.CompletedTask; });

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
			F.LogException(ex);
		}

		return result;
	}
}
