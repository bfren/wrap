// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap;

public static partial class MaybeExtensions
{
	/// <inheritdoc cref="M.Switch{T}(Maybe{T}, Action, Action{T})"/>
	public static void Switch<T>(this Maybe<T> @this, Action none, Action<T> some) =>
		M.Switch(@this, none, some);

	/// <inheritdoc cref="M.Switch{T}(Maybe{T}, Action, Action{T})"/>
	public static Task SwitchAsync<T>(this Maybe<T> @this, Action none, Func<T, Task> some) =>
		M.SwitchAsync(@this, none, some);

	/// <inheritdoc cref="M.Switch{T}(Maybe{T}, Action, Action{T})"/>
	public static Task SwitchAsync<T>(this Maybe<T> @this, Func<Task> none, Action<T> some) =>
		M.SwitchAsync(@this, none, some);

	/// <inheritdoc cref="M.Switch{T}(Maybe{T}, Action, Action{T})"/>
	public static Task SwitchAsync<T>(this Maybe<T> @this, Func<Task> none, Func<T, Task> some) =>
		M.SwitchAsync(@this, none, some);

	/// <inheritdoc cref="M.Switch{T}(Maybe{T}, Action, Action{T})"/>
	public static Task SwitchAsync<T>(this Task<Maybe<T>> @this, Func<Task> none, Func<T, Task> some) =>
		M.SwitchAsync(@this, none, some);

	/// <inheritdoc cref="M.Switch{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static TReturn Switch<T, TReturn>(this Maybe<T> @this, TReturn none, Func<T, TReturn> some) =>
		M.Switch(@this, none, some);

	/// <inheritdoc cref="M.Switch{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static TReturn Switch<T, TReturn>(this Maybe<T> @this, Func<TReturn> none, Func<T, TReturn> some) =>
		M.Switch(@this, none, some);

	/// <inheritdoc cref="M.Switch{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> SwitchAsync<T, TReturn>(this Maybe<T> @this, TReturn none, Func<T, Task<TReturn>> some) =>
		M.SwitchAsync(@this, none, some);

	/// <inheritdoc cref="M.Switch{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> SwitchAsync<T, TReturn>(this Maybe<T> @this, Func<TReturn> none, Func<T, Task<TReturn>> some) =>
		M.SwitchAsync(@this, none, some);

	/// <inheritdoc cref="M.Switch{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> SwitchAsync<T, TReturn>(this Maybe<T> @this, Func<Task<TReturn>> none, Func<T, TReturn> some) =>
		M.SwitchAsync(@this, none, some);

	/// <inheritdoc cref="M.Switch{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> SwitchAsync<T, TReturn>(this Maybe<T> @this, Func<Task<TReturn>> none, Func<T, Task<TReturn>> some) =>
		M.SwitchAsync(@this, none, some);

	/// <inheritdoc cref="M.Switch{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> SwitchAsync<T, TReturn>(this Task<Maybe<T>> @this, TReturn none, Func<T, TReturn> some) =>
		M.SwitchAsync(@this, none, some);

	/// <inheritdoc cref="M.Switch{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> SwitchAsync<T, TReturn>(this Task<Maybe<T>> @this, TReturn none, Func<T, Task<TReturn>> some) =>
		M.SwitchAsync(@this, none, some);

	/// <inheritdoc cref="M.Switch{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> SwitchAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<TReturn> none, Func<T, TReturn> some) =>
		M.SwitchAsync(@this, none, some);

	/// <inheritdoc cref="M.Switch{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> SwitchAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<TReturn> none, Func<T, Task<TReturn>> some) =>
		M.SwitchAsync(@this, none, some);

	/// <inheritdoc cref="M.Switch{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> SwitchAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<Task<TReturn>> none, Func<T, TReturn> some) =>
		M.SwitchAsync(@this, none, some);

	/// <inheritdoc cref="M.Switch{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> SwitchAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<Task<TReturn>> none, Func<T, Task<TReturn>> some) =>
		M.SwitchAsync(@this, none, some);
}
