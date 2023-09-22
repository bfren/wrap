// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Monadic;

public static partial class ResultExtensions
{
	/// <inheritdoc cref="R.Switch{T}(Result{T}, Action{ErrValue}, Action{T})"/>
	public static void Switch<T>(this Result<T> @this, Action<ErrValue> err, Action<T> ok) =>
		R.Switch(@this, err, ok);

	/// <inheritdoc cref="R.Switch{T}(Result{T}, Action{ErrValue}, Action{T})"/>
	public static Task SwitchAsync<T>(this Result<T> @this, Func<ErrValue, Task> err, Func<T, Task> ok) =>
		R.SwitchAsync(@this, err, ok);

	/// <inheritdoc cref="R.Switch{T}(Result{T}, Action{ErrValue}, Action{T})"/>
	public static Task SwitchAsync<T>(this Task<Result<T>> @this, Func<ErrValue, Task> err, Func<T, Task> ok) =>
		R.SwitchAsync(@this, err, ok);

	/// <inheritdoc cref="R.Switch{T, TReturn}(Result{T}, Func{ErrValue,TReturn}, Func{T, TReturn})"/>
	public static TReturn Switch<T, TReturn>(this Result<T> @this, Func<ErrValue, TReturn> err, Func<T, TReturn> ok) =>
		R.Switch(@this, err, ok);

	/// <inheritdoc cref="R.Switch{T, TReturn}(Result{T}, Func{ErrValue,TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> SwitchAsync<T, TReturn>(this Result<T> @this, Func<ErrValue, TReturn> err, Func<T, Task<TReturn>> ok) =>
		R.SwitchAsync(@this, err, ok);

	/// <inheritdoc cref="R.Switch{T, TReturn}(Result{T}, Func{ErrValue,TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> SwitchAsync<T, TReturn>(this Result<T> @this, Func<ErrValue, Task<TReturn>> err, Func<T, TReturn> ok) =>
		R.SwitchAsync(@this, err, ok);

	/// <inheritdoc cref="R.Switch{T, TReturn}(Result{T}, Func{ErrValue,TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> SwitchAsync<T, TReturn>(this Result<T> @this, Func<ErrValue, Task<TReturn>> err, Func<T, Task<TReturn>> ok) =>
		R.SwitchAsync(@this, err, ok);

	/// <inheritdoc cref="R.Switch{T, TReturn}(Result{T}, Func{ErrValue,TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> SwitchAsync<T, TReturn>(this Task<Result<T>> @this, Func<ErrValue, TReturn> err, Func<T, TReturn> ok) =>
		R.SwitchAsync(@this, err, ok);

	/// <inheritdoc cref="R.Switch{T, TReturn}(Result{T}, Func{ErrValue,TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> SwitchAsync<T, TReturn>(this Task<Result<T>> @this, Func<ErrValue, TReturn> err, Func<T, Task<TReturn>> ok) =>
		R.SwitchAsync(@this, err, ok);

	/// <inheritdoc cref="R.Switch{T, TReturn}(Result{T}, Func{ErrValue,TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> SwitchAsync<T, TReturn>(this Task<Result<T>> @this, Func<ErrValue, Task<TReturn>> err, Func<T, TReturn> ok) =>
		R.SwitchAsync(@this, err, ok);

	/// <inheritdoc cref="R.Switch{T, TReturn}(Result{T}, Func{ErrValue,TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> SwitchAsync<T, TReturn>(this Task<Result<T>> @this, Func<ErrValue, Task<TReturn>> err, Func<T, Task<TReturn>> ok) =>
		R.SwitchAsync(@this, err, ok);
}
