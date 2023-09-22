// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Run <paramref name="none"/> or <paramref name="some"/> depending on the value of <paramref name="this"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="none">Audit function to run when <paramref name="this"/> is <see cref="None"/>.</param>
	/// <param name="some">Audit function to run when <paramref name="this"/> is <see cref="Some{T}"/>.</param>
	/// <returns>The original value of <paramref name="this"/>.</returns>
	public static Maybe<T> Audit<T>(this Maybe<T> @this, Action? none = null, Action<T>? some = null)
	{
		try
		{
			if (@this is Maybe<T>.None y && none is not null)
			{
				none();
			}
			else if (@this is Some<T> x && some is not null)
			{
				some(x.Value);
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
			F.LogException(ex);
		}

		return @this;
	}

	/// <inheritdoc cref="Audit{T}(Maybe{T}, Action?, Action{T}?)"/>
	public static Task<Maybe<T>> AuditAsync<T>(this Maybe<T> @this, Func<Task>? none = null, Func<T, Task>? some = null) =>
		AuditAsync(@this.AsTask(), none, some);

	/// <inheritdoc cref="Audit{T}(Maybe{T}, Action?, Action{T}?)"/>
	public static Task<Maybe<T>> AuditAsync<T>(this Task<Maybe<T>> @this, Action? none = null, Action<T>? some = null) =>
		AuditAsync(@this, () => { none?.Invoke(); return Task.CompletedTask; }, x => { some?.Invoke(x); return Task.CompletedTask; });

	/// <inheritdoc cref="Audit{T}(Maybe{T}, Action?, Action{T}?)"/>
	public static async Task<Maybe<T>> AuditAsync<T>(this Task<Maybe<T>> @this, Func<Task>? none = null, Func<T, Task>? some = null)
	{
		var result = await @this;

		try
		{
			if (result is Maybe<T>.None y && none is not null)
			{
				await none();
			}
			else if (result is Some<T> x && some is not null)
			{
				await some(x.Value);
			}
		}
		catch (Exception ex)
		{
			F.LogException(ex);
		}

		return result;
	}

	/// <inheritdoc cref="Audit{T}(Maybe{T}, Action{Maybe{T}})"/>
	public static Task<Maybe<T>> AuditAsync<T>(this Maybe<T> @this, Func<Maybe<T>, Task> either) =>
		AuditAsync(@this.AsTask(), either);

	/// <inheritdoc cref="Audit{T}(Maybe{T}, Action{Maybe{T}})"/>
	public static Task<Maybe<T>> AuditAsync<T>(this Task<Maybe<T>> @this, Action<Maybe<T>> either) =>
		AuditAsync(@this, x => { either(x); return Task.CompletedTask; });

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
			F.LogException(ex);
		}

		return result;
	}
}
