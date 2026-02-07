// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ResultExtensions
{
	/// <inheritdoc cref="R.Match{T}(Result{T}, Action{FailureValue}, Action{T})"/>
	public static void Match<T>(this Result<T> @this, Action<FailureValue> fFail, Action<T> fOk) =>
		R.Match(@this, fFail, fOk);

	/// <inheritdoc cref="R.Match{T}(Result{T}, Action{FailureValue}, Action{T})"/>
	public static Task MatchAsync<T>(this Result<T> @this, Func<FailureValue, Task> fFail, Func<T, Task> fOk) =>
		R.MatchAsync(@this, fFail, fOk);

	/// <inheritdoc cref="R.Match{T}(Result{T}, Action{FailureValue}, Action{T})"/>
	public static Task MatchAsync<T>(this Task<Result<T>> @this, Func<FailureValue, Task> fFail, Func<T, Task> fOk) =>
		R.MatchAsync(@this, fFail, fOk);

	/// <inheritdoc cref="R.Match{T, TReturn}(Result{T}, Func{FailureValue,TReturn}, Func{T, TReturn})"/>
	public static TReturn Match<T, TReturn>(this Result<T> @this, Func<FailureValue, TReturn> fFail, Func<T, TReturn> fOk) =>
		R.Match(@this, fFail, fOk);

	/// <inheritdoc cref="R.Match{T, TReturn}(Result{T}, Func{FailureValue,TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Result<T> @this, Func<FailureValue, TReturn> fFail, Func<T, Task<TReturn>> fOk) =>
		R.MatchAsync(@this, fFail, fOk);

	/// <inheritdoc cref="R.Match{T, TReturn}(Result{T}, Func{FailureValue,TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Result<T> @this, Func<FailureValue, Task<TReturn>> fFail, Func<T, TReturn> fOk) =>
		R.MatchAsync(@this, fFail, fOk);

	/// <inheritdoc cref="R.Match{T, TReturn}(Result{T}, Func{FailureValue,TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Result<T> @this, Func<FailureValue, Task<TReturn>> fFail, Func<T, Task<TReturn>> fOk) =>
		R.MatchAsync(@this, fFail, fOk);

	/// <inheritdoc cref="R.Match{T, TReturn}(Result{T}, Func{FailureValue,TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Task<Result<T>> @this, Func<FailureValue, TReturn> fFail, Func<T, TReturn> fOk) =>
		R.MatchAsync(@this, fFail, fOk);

	/// <inheritdoc cref="R.Match{T, TReturn}(Result{T}, Func{FailureValue,TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Task<Result<T>> @this, Func<FailureValue, TReturn> fFail, Func<T, Task<TReturn>> fOk) =>
		R.MatchAsync(@this, fFail, fOk);

	/// <inheritdoc cref="R.Match{T, TReturn}(Result{T}, Func{FailureValue,TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Task<Result<T>> @this, Func<FailureValue, Task<TReturn>> fFail, Func<T, TReturn> fOk) =>
		R.MatchAsync(@this, fFail, fOk);

	/// <inheritdoc cref="R.Match{T, TReturn}(Result{T}, Func{FailureValue,TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Task<Result<T>> @this, Func<FailureValue, Task<TReturn>> fFail, Func<T, Task<TReturn>> fOk) =>
		R.MatchAsync(@this, fFail, fOk);
}
