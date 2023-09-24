// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap;

public static partial class ResultExtensions
{
	/// <inheritdoc cref="R.Match{T}(Result{T}, Action{ErrValue}, Action{T})"/>
	public static void Match<T>(this Result<T> @this, Action<ErrValue> err, Action<T> ok) =>
		R.Match(@this, err, ok);

	/// <inheritdoc cref="R.Match{T}(Result{T}, Action{ErrValue}, Action{T})"/>
	public static Task MatchAsync<T>(this Result<T> @this, Func<ErrValue, Task> err, Func<T, Task> ok) =>
		R.MatchAsync(@this, err, ok);

	/// <inheritdoc cref="R.Match{T}(Result{T}, Action{ErrValue}, Action{T})"/>
	public static Task MatchAsync<T>(this Task<Result<T>> @this, Func<ErrValue, Task> err, Func<T, Task> ok) =>
		R.MatchAsync(@this, err, ok);

	/// <inheritdoc cref="R.Match{T, TReturn}(Result{T}, Func{ErrValue,TReturn}, Func{T, TReturn})"/>
	public static TReturn Match<T, TReturn>(this Result<T> @this, Func<ErrValue, TReturn> err, Func<T, TReturn> ok) =>
		R.Match(@this, err, ok);

	/// <inheritdoc cref="R.Match{T, TReturn}(Result{T}, Func{ErrValue,TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Result<T> @this, Func<ErrValue, TReturn> err, Func<T, Task<TReturn>> ok) =>
		R.MatchAsync(@this, err, ok);

	/// <inheritdoc cref="R.Match{T, TReturn}(Result{T}, Func{ErrValue,TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Result<T> @this, Func<ErrValue, Task<TReturn>> err, Func<T, TReturn> ok) =>
		R.MatchAsync(@this, err, ok);

	/// <inheritdoc cref="R.Match{T, TReturn}(Result{T}, Func{ErrValue,TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Result<T> @this, Func<ErrValue, Task<TReturn>> err, Func<T, Task<TReturn>> ok) =>
		R.MatchAsync(@this, err, ok);

	/// <inheritdoc cref="R.Match{T, TReturn}(Result{T}, Func{ErrValue,TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Task<Result<T>> @this, Func<ErrValue, TReturn> err, Func<T, TReturn> ok) =>
		R.MatchAsync(@this, err, ok);

	/// <inheritdoc cref="R.Match{T, TReturn}(Result{T}, Func{ErrValue,TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Task<Result<T>> @this, Func<ErrValue, TReturn> err, Func<T, Task<TReturn>> ok) =>
		R.MatchAsync(@this, err, ok);

	/// <inheritdoc cref="R.Match{T, TReturn}(Result{T}, Func{ErrValue,TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Task<Result<T>> @this, Func<ErrValue, Task<TReturn>> err, Func<T, TReturn> ok) =>
		R.MatchAsync(@this, err, ok);

	/// <inheritdoc cref="R.Match{T, TReturn}(Result{T}, Func{ErrValue,TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Task<Result<T>> @this, Func<ErrValue, Task<TReturn>> err, Func<T, Task<TReturn>> ok) =>
		R.MatchAsync(@this, err, ok);
}
