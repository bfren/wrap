// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Monadic;

public static partial class MaybeExtensions
{
	public static void Switch<T>(this Maybe<T> @this, Action none, Action<T> some) =>
		M.Switch(@this, none, some);

	public static Task SwitchAsync<T>(this Maybe<T> @this, Func<Task> none, Func<T, Task> some) =>
		M.SwitchAsync(@this, none, some);

	public static Task SwitchAsync<T>(this Task<Maybe<T>> @this, Func<Task> none, Func<T, Task> some) =>
		M.SwitchAsync(@this, none, some);

	public static TReturn Switch<T, TReturn>(this Maybe<T> @this, TReturn none, Func<T, TReturn> some) =>
		M.Switch(@this, none, some);

	public static TReturn Switch<T, TReturn>(this Maybe<T> @this, Func<TReturn> none, Func<T, TReturn> some) =>
		M.Switch(@this, none, some);

	public static Task<TReturn> SwitchAsync<T, TReturn>(this Maybe<T> @this, TReturn none, Func<T, Task<TReturn>> some) =>
		M.SwitchAsync(@this, none, some);

	public static Task<TReturn> SwitchAsync<T, TReturn>(this Maybe<T> @this, Func<TReturn> none, Func<T, Task<TReturn>> some) =>
		M.SwitchAsync(@this, none, some);

	public static Task<TReturn> SwitchAsync<T, TReturn>(this Maybe<T> @this, Func<Task<TReturn>> none, Func<T, TReturn> some) =>
		M.SwitchAsync(@this, none, some);

	public static Task<TReturn> SwitchAsync<T, TReturn>(this Maybe<T> @this, Func<Task<TReturn>> none, Func<T, Task<TReturn>> some) =>
		M.SwitchAsync(@this, none, some);

	public static Task<TReturn> SwitchAsync<T, TReturn>(this Task<Maybe<T>> @this, TReturn none, Func<T, TReturn> some) =>
		M.SwitchAsync(@this, none, some);

	public static Task<TReturn> SwitchAsync<T, TReturn>(this Task<Maybe<T>> @this, TReturn none, Func<T, Task<TReturn>> some) =>
		M.SwitchAsync(@this, none, some);

	public static Task<TReturn> SwitchAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<TReturn> none, Func<T, TReturn> some) =>
		M.SwitchAsync(@this, none, some);

	public static Task<TReturn> SwitchAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<TReturn> none, Func<T, Task<TReturn>> some) =>
		M.SwitchAsync(@this, none, some);

	public static Task<TReturn> SwitchAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<Task<TReturn>> none, Func<T, TReturn> some) =>
		M.SwitchAsync(@this, none, some);

	public static Task<TReturn> SwitchAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<Task<TReturn>> none, Func<T, Task<TReturn>> some) =>
		M.SwitchAsync(@this, none, some);
}
