// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class MaybeExtensions
{
	/// <inheritdoc cref="Audit{T}(Maybe{T}, Action?, Action{T}?)"/>
	public static Maybe<T> Audit<T>(this Maybe<T> @this, Action fNone) =>
		Audit(@this, fNone, null);

	/// <inheritdoc cref="Audit{T}(Maybe{T}, Action?, Action{T}?)"/>
	public static Maybe<T> Audit<T>(this Maybe<T> @this, Action<T> fSome) =>
		Audit(@this, null, fSome);

	/// <summary>
	/// Run <paramref name="fNone"/> or <paramref name="fSome"/> depending on the value of <paramref name="this"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="fNone">Audit function to run when <paramref name="this"/> is <see cref="None"/>.</param>
	/// <param name="fSome">Audit function to run when <paramref name="this"/> is <see cref="Some{T}"/>.</param>
	/// <returns>The original value of <paramref name="this"/>.</returns>
	public static Maybe<T> Audit<T>(this Maybe<T> @this, Action? fNone, Action<T>? fSome)
	{
		try
		{
			if (@this is Maybe<T>.NoneImpl && fNone is not null)
			{
				fNone();
			}
			else if (@this is Some<T> x && fSome is not null)
			{
				fSome(x.Value);
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
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="either">Audit function to run.</param>
	/// <returns>The original value of <paramref name="this"/>.</returns>
	public static Maybe<T> Audit<T>(this Maybe<T> @this, Action<Maybe<T>> either)
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

	/// <inheritdoc cref="Audit{T}(Maybe{T}, Action?, Action{T}?)"/>
	public static Task<Maybe<T>> AuditAsync<T>(this Maybe<T> @this, Func<Task> fNone) =>
		AuditAsync(@this, fNone, null);

	/// <inheritdoc cref="Audit{T}(Maybe{T}, Action?, Action{T}?)"/>
	public static Task<Maybe<T>> AuditAsync<T>(this Maybe<T> @this, Func<T, Task> fSome) =>
		AuditAsync(@this, null, fSome);

	/// <inheritdoc cref="Audit{T}(Maybe{T}, Action?, Action{T}?)"/>
	public static Task<Maybe<T>> AuditAsync<T>(this Maybe<T> @this, Func<Task>? fNone, Func<T, Task>? fSome) =>
		AuditAsync(@this.AsTask(), fNone, fSome);

	/// <inheritdoc cref="Audit{T}(Maybe{T}, Action?, Action{T}?)"/>
	public static Task<Maybe<T>> AuditAsync<T>(this Task<Maybe<T>> @this, Action fNone) =>
		AuditAsync(@this, fNone, null);

	/// <inheritdoc cref="Audit{T}(Maybe{T}, Action?, Action{T}?)"/>
	public static Task<Maybe<T>> AuditAsync<T>(this Task<Maybe<T>> @this, Action<T> fSome) =>
		AuditAsync(@this, null, fSome);

	/// <inheritdoc cref="Audit{T}(Maybe{T}, Action?, Action{T}?)"/>
	public static Task<Maybe<T>> AuditAsync<T>(this Task<Maybe<T>> @this, Action? fNone, Action<T>? fSome) =>
		AuditAsync(@this, async () => fNone?.Invoke(), async x => fSome?.Invoke(x));

	/// <inheritdoc cref="Audit{T}(Maybe{T}, Action?, Action{T}?)"/>
	public static Task<Maybe<T>> AuditAsync<T>(this Task<Maybe<T>> @this, Func<Task> fNone) =>
		AuditAsync(@this, fNone, null);

	/// <inheritdoc cref="Audit{T}(Maybe{T}, Action?, Action{T}?)"/>
	public static Task<Maybe<T>> AuditAsync<T>(this Task<Maybe<T>> @this, Func<T, Task> fSome) =>
		AuditAsync(@this, null, fSome);

	/// <inheritdoc cref="Audit{T}(Maybe{T}, Action?, Action{T}?)"/>
	public static async Task<Maybe<T>> AuditAsync<T>(this Task<Maybe<T>> @this, Func<Task>? fNone, Func<T, Task>? fSome)
	{
		var result = await @this;

		try
		{
			if (result is Maybe<T>.NoneImpl && fNone is not null)
			{
				await fNone();
			}
			else if (result is Some<T> x && fSome is not null)
			{
				await fSome(x.Value);
			}
		}
		catch (Exception ex)
		{
			F.LogException?.Invoke(ex);
		}

		return result;
	}

	/// <inheritdoc cref="Audit{T}(Maybe{T}, Action{Maybe{T}})"/>
	public static Task<Maybe<T>> AuditAsync<T>(this Maybe<T> @this, Func<Maybe<T>, Task> either) =>
		AuditAsync(@this.AsTask(), either);

	/// <inheritdoc cref="Audit{T}(Maybe{T}, Action{Maybe{T}})"/>
	public static Task<Maybe<T>> AuditAsync<T>(this Task<Maybe<T>> @this, Action<Maybe<T>> either) =>
		AuditAsync(@this, either: async x => either(x));

	/// <inheritdoc cref="Audit{T}(Maybe{T}, Action{Maybe{T}})"/>
	public static async Task<Maybe<T>> AuditAsync<T>(this Task<Maybe<T>> @this, Func<Maybe<T>, Task> either)
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
