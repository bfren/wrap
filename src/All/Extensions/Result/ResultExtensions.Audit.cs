// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ResultExtensions
{
	/// <summary>
	/// Run <paramref name="fFail"/> or <paramref name="fOk"/> depending on the value of <paramref name="this"/>.
	/// </summary>
	/// <remarks>
	/// Exceptions thrown by the audit functions are ignored, so auditing never changes the result.
	/// Catch them inside the function if you need to know about them.
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="fFail">Audit function to run when <paramref name="this"/> is <see cref="Failure"/>.</param>
	/// <param name="fOk">Audit function to run when <paramref name="this"/> is <see cref="Ok{T}"/>.</param>
	/// <returns>The original value of <paramref name="this"/>.</returns>
	public static Result<T> Audit<T>(this Result<T> @this, Action<Failure>? fFail, Action<T>? fOk)
	{
		try
		{
			if (@this is Failure y && fFail is not null)
			{
				fFail(y);
			}
			else if (@this is Ok<T> x && fOk is not null)
			{
				fOk(x.Value);
			}
		}
		catch
		{
			// Audit functions must not change the result, so their exceptions are ignored
		}

		return @this;
	}

	/// <summary>
	/// Run <paramref name="f"/> with the value of <paramref name="this"/>.
	/// </summary>
	/// <remarks>
	/// Exceptions thrown by <paramref name="f"/> are ignored, so auditing never changes the result.
	/// Catch them inside the function if you need to know about them.
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="f">Audit function to run.</param>
	/// <returns>The original value of <paramref name="this"/>.</returns>
	public static Result<T> Audit<T>(this Result<T> @this, Action<Result<T>> f)
	{
		try
		{
			f(@this);
		}
		catch
		{
			// Audit functions must not change the result, so their exceptions are ignored
		}

		return @this;
	}

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{Failure}?, Action{T}?)"/>
	public static async Task<Result<T>> AuditAsync<T>(this Result<T> @this, Func<Failure, Task>? fFail, Func<T, Task>? fOk)
	{
		try
		{
			if (@this is Failure y && fFail is not null)
			{
				await fFail(y);
			}
			else if (@this is Ok<T> x && fOk is not null)
			{
				await fOk(x.Value);
			}
		}
		catch
		{
			// Audit functions must not change the result, so their exceptions are ignored
		}

		return @this;
	}

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{Failure}?, Action{T}?)"/>
	public static Task<Result<T>> AuditAsync<T>(this Task<Result<T>> @this, Action<Failure>? fFail, Action<T>? fOk) =>
		@this.Then(x => x.Audit(fFail, fOk));

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{Failure}?, Action{T}?)"/>
	public static Task<Result<T>> AuditAsync<T>(this Task<Result<T>> @this, Func<Failure, Task>? fFail, Func<T, Task>? fOk) =>
		@this.Then(x => x.AuditAsync(fFail, fOk));

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{Result{T}})"/>
	public static async Task<Result<T>> AuditAsync<T>(this Result<T> @this, Func<Result<T>, Task> f)
	{
		try
		{
			await f(@this);
		}
		catch
		{
			// Audit functions must not change the result, so their exceptions are ignored
		}

		return @this;
	}

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{Result{T}})"/>
	public static Task<Result<T>> AuditAsync<T>(this Task<Result<T>> @this, Action<Result<T>> f) =>
		@this.Then(x => x.Audit(f));

	/// <inheritdoc cref="Audit{T}(Result{T}, Action{Result{T}})"/>
	public static Task<Result<T>> AuditAsync<T>(this Task<Result<T>> @this, Func<Result<T>, Task> f) =>
		@this.Then(x => x.AuditAsync(f));
}
