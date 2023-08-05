// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;
using Monadic.Exceptions;

namespace Monadic;

public static partial class M
{
	public static void Switch<T>(Maybe<T> maybe, Action none, Action<T> some)
	{
		if (maybe is Maybe<T>.None)
		{
			none();
		}
		else if (maybe is Some<T> x)
		{
			some(x.Value);
		}
		else
		{
			throw new InvalidMaybeTypeException(maybe.GetType());
		}
	}

	public static Task SwitchAsync<T>(Maybe<T> maybe, Func<Task> none, Func<T, Task> some) =>
		Switch(maybe, none, some);

	public static async Task SwitchAsync<T>(Task<Maybe<T>> maybe, Func<Task> none, Func<T, Task> some) =>
		await Switch(await maybe, none, some);

	public static TReturn Switch<T, TReturn>(Maybe<T> maybe, TReturn none, Func<T, TReturn> some) =>
		Switch(maybe, () => none, some);

	public static TReturn Switch<T, TReturn>(Maybe<T> maybe, Func<TReturn> none, Func<T, TReturn> some) =>
		maybe switch
		{
			Maybe<T>.None =>
				none(),

			Some<T> x =>
				some(x.Value),

			_ =>
				throw new InvalidMaybeTypeException(maybe.GetType())
		};

	public static Task<TReturn> SwitchAsync<T, TReturn>(Maybe<T> maybe, TReturn none, Func<T, Task<TReturn>> some) =>
		Switch(maybe, () => Task.FromResult(none), some);

	public static Task<TReturn> SwitchAsync<T, TReturn>(Maybe<T> maybe, Func<TReturn> none, Func<T, Task<TReturn>> some) =>
		Switch(maybe, () => Task.FromResult(none()), some);

	public static Task<TReturn> SwitchAsync<T, TReturn>(Maybe<T> maybe, Func<Task<TReturn>> none, Func<T, Task<TReturn>> some) =>
		Switch(maybe, none, some);

	public static Task<TReturn> SwitchAsync<T, TReturn>(Task<Maybe<T>> maybe, TReturn none, Func<T, TReturn> some) =>
		SwitchAsync(maybe, () => Task.FromResult(none), x => Task.FromResult(some(x)));

	public static Task<TReturn> SwitchAsync<T, TReturn>(Task<Maybe<T>> maybe, TReturn none, Func<T, Task<TReturn>> some) =>
		SwitchAsync(maybe, () => Task.FromResult(none), some);

	public static Task<TReturn> SwitchAsync<T, TReturn>(Task<Maybe<T>> maybe, Func<TReturn> none, Func<T, Task<TReturn>> some) =>
		SwitchAsync(maybe, () => Task.FromResult(none()), some);

	public static async Task<TReturn> SwitchAsync<T, TReturn>(Task<Maybe<T>> maybe, Func<Task<TReturn>> none, Func<T, Task<TReturn>> some) =>
		await Switch(await maybe, none, some);
}
