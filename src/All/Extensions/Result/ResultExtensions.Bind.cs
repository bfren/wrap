// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ResultExtensions
{
	/// <summary>
	/// Run <paramref name="f"/> when <paramref name="this"/> is <see cref="Ok{T}"/>.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="f">Function to convert a <typeparamref name="T"/> object to a <typeparamref name="TReturn"/> object.</param>
	/// <returns><see cref="Result{T}"/> object returned by <paramref name="f"/> or <see cref="Failure"/>.</returns>
	public static Result<TReturn> Bind<T, TReturn>(this Result<T> @this, Func<T, Result<TReturn>> f) =>
		R.Match(@this,
			fFail: R.Fail<TReturn>,
			fOk: f
		);

	/// <inheritdoc cref="Bind{T, TReturn}(Result{T}, Func{T, Result{TReturn}})"/>
	public static Task<Result<TReturn>> BindAsync<T, TReturn>(this Result<T> @this, Func<T, Task<Result<TReturn>>> f) =>
		R.MatchAsync(@this,
			fFail: R.Fail<TReturn>,
			fOk: f
		);

	/// <inheritdoc cref="Bind{T, TReturn}(Result{T}, Func{T, Result{TReturn}})"/>
	public static Task<Result<TReturn>> BindAsync<T, TReturn>(this Task<Result<T>> @this, Func<T, Result<TReturn>> f) =>
		R.MatchAsync(@this,
			fFail: R.Fail<TReturn>,
			fOk: f
		);

	/// <inheritdoc cref="Bind{T, TReturn}(Result{T}, Func{T, Result{TReturn}})"/>
	public static Task<Result<TReturn>> BindAsync<T, TReturn>(this Task<Result<T>> @this, Func<T, Task<Result<TReturn>>> f) =>
		R.MatchAsync(@this,
			fFail: R.Fail<TReturn>,
			fOk: f
		);
}
