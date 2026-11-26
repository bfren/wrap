// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Run <paramref name="fNone"/> or <paramref name="fSome"/> depending on the value of <paramref name="this"/>.
	/// </summary>
	/// <remarks>
	/// Exceptions thrown by the audit functions are ignored, so auditing never changes the result.
	/// Catch them inside the function if you need to know about them.
	/// </remarks>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="fNone">Audit function to run when <paramref name="this"/> is <see cref="None"/>.</param>
	/// <param name="fSome">Audit function to run when <paramref name="this"/> is <see cref="Some{T}"/>.</param>
	/// <returns>The original value of <paramref name="this"/>.</returns>
	public static Maybe<T> Audit<T>(this Maybe<T> @this, Action? fNone, Action<T>? fSome)
	{
		try
		{
			if (@this is None && fNone is not null)
			{
				fNone();
			}
			else if (@this is Some<T> x && fSome is not null)
			{
				fSome(x.Value);
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
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="f">Audit function to run.</param>
	/// <returns>The original value of <paramref name="this"/>.</returns>
	public static Maybe<T> Audit<T>(this Maybe<T> @this, Action<Maybe<T>> f)
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

	/// <inheritdoc cref="Audit{T}(Maybe{T}, Action?, Action{T}?)"/>
	public static async Task<Maybe<T>> AuditAsync<T>(this Maybe<T> @this, Func<Task>? fNone, Func<T, Task>? fSome)
	{
		try
		{
			if (@this is None && fNone is not null)
			{
				await fNone();
			}
			else if (@this is Some<T> x && fSome is not null)
			{
				await fSome(x.Value);
			}
		}
		catch
		{
			// Audit functions must not change the result, so their exceptions are ignored
		}

		return @this;
	}

	/// <inheritdoc cref="Audit{T}(Maybe{T}, Action?, Action{T}?)"/>
	public static Task<Maybe<T>> AuditAsync<T>(this Task<Maybe<T>> @this, Action? fNone, Action<T>? fSome) =>
		@this.Then(x => x.Audit(fNone, fSome));

	/// <inheritdoc cref="Audit{T}(Maybe{T}, Action?, Action{T}?)"/>
	public static Task<Maybe<T>> AuditAsync<T>(this Task<Maybe<T>> @this, Func<Task>? fNone, Func<T, Task>? fSome) =>
		@this.Then(x => x.AuditAsync(fNone, fSome));

	/// <inheritdoc cref="Audit{T}(Maybe{T}, Action{Maybe{T}})"/>
	public static async Task<Maybe<T>> AuditAsync<T>(this Maybe<T> @this, Func<Maybe<T>, Task> f)
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

	/// <inheritdoc cref="Audit{T}(Maybe{T}, Action{Maybe{T}})"/>
	public static Task<Maybe<T>> AuditAsync<T>(this Task<Maybe<T>> @this, Action<Maybe<T>> f) =>
		@this.Then(x => x.Audit(f));

	/// <inheritdoc cref="Audit{T}(Maybe{T}, Action{Maybe{T}})"/>
	public static Task<Maybe<T>> AuditAsync<T>(this Task<Maybe<T>> @this, Func<Maybe<T>, Task> f) =>
		@this.Then(x => x.AuditAsync(f));
}
