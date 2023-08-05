// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;
using Monadic.Exceptions;

namespace Monadic;

public static partial class R
{
	public static void Switch<T>(Result<T> result, Action<Exception> err, Action<T> ok)
	{
		if (result is Result<T>.Err x)
		{
			err(x.Value);
		}
		else if (result is Ok<T> y)
		{
			ok(y.Value);
		}
		else
		{
			throw new InvalidResultTypeException(result.GetType());
		}
	}

	public static Task SwitchAsync<T>(Result<T> result, Action<Exception> err, Func<T, Task> ok) =>
		Switch(result, x => { err(x); return Task.CompletedTask; }, ok);

	public static Task SwitchAsync<T>(Result<T> result, Func<Exception, Task> err, Action<T> ok) =>
		Switch(result, err, x => { ok(x); return Task.CompletedTask; });

	public static Task SwitchAsync<T>(Result<T> result, Func<Exception, Task> err, Func<T, Task> ok) =>
		Switch(result, err, ok);

	public static async Task SwitchAsync<T>(Task<Result<T>> result, Func<Exception, Task> err, Func<T, Task> ok) =>
		await Switch(await result, err, ok);

	public static TReturn Switch<T, TReturn>(Result<T> result, Func<Exception, TReturn> err, Func<T, TReturn> ok) =>
		result switch
		{
			Result<T>.Err x =>
				err(x.Value),

			Ok<T> y =>
				ok(y.Value),

			_ =>
				throw new InvalidResultTypeException(result.GetType())
		};

	public static Task<TReturn> SwitchAsync<T, TReturn>(Result<T> result, Func<Exception, TReturn> err, Func<T, Task<TReturn>> ok) =>
		Switch(result, x => Task.FromResult(err(x)), ok);

	public static Task<TReturn> SwitchAsync<T, TReturn>(Result<T> result, Func<Exception, Task<TReturn>> err, Func<T, TReturn> ok) =>
		Switch(result, err, x => Task.FromResult(ok(x)));

	public static Task<TReturn> SwitchAsync<T, TReturn>(Result<T> result, Func<Exception, Task<TReturn>> err, Func<T, Task<TReturn>> ok) =>
		Switch(result, err, ok);

	public static Task<TReturn> SwitchAsync<T, TReturn>(Task<Result<T>> result, Func<Exception, TReturn> err, Func<T, TReturn> ok) =>
		SwitchAsync(result, x => Task.FromResult(err(x)), x => Task.FromResult(ok(x)));

	public static Task<TReturn> SwitchAsync<T, TReturn>(Task<Result<T>> result, Func<Exception, TReturn> err, Func<T, Task<TReturn>> ok) =>
		SwitchAsync(result, x => Task.FromResult(err(x)), ok);

	public static Task<TReturn> SwitchAsync<T, TReturn>(Task<Result<T>> result, Func<Exception, Task<TReturn>> err, Func<T, TReturn> ok) =>
		SwitchAsync(result, err, x => Task.FromResult(ok(x)));

	public static async Task<TReturn> SwitchAsync<T, TReturn>(Task<Result<T>> result, Func<Exception, Task<TReturn>> err, Func<T, Task<TReturn>> ok) =>
		await Switch(await result, err, ok);
}
