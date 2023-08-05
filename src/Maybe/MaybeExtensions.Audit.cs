// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Monadic;

public static partial class MaybeExtensions
{
	public static Maybe<T> Audit<T>(this Maybe<T> @this, Action? none, Action<T>? some)
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

	public static Task<Maybe<T>> AuditAsync<T>(this Maybe<T> @this, Func<Task>? none, Func<T, Task>? some) =>
		AuditAsync(@this.AsTask(), none, some);

	public static Task<Maybe<T>> AuditAsync<T>(this Task<Maybe<T>> @this, Action? none, Action<T>? some) =>
		AuditAsync(@this, () => { none?.Invoke(); return Task.CompletedTask; }, x => { some?.Invoke(x); return Task.CompletedTask; });

	public static async Task<Maybe<T>> AuditAsync<T>(this Task<Maybe<T>> @this, Func<Task>? none, Func<T, Task>? some)
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

	public static Task<Maybe<T>> AuditAsync<T>(this Maybe<T> @this, Func<Maybe<T>, Task> either) =>
		AuditAsync(@this.AsTask(), either);

	public static Task<Maybe<T>> AuditAsync<T>(this Task<Maybe<T>> @this, Action<Maybe<T>> either) =>
		AuditAsync(@this, x => { either(x); return Task.CompletedTask; });

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
