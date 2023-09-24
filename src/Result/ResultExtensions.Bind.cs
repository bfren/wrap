// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap;

public static partial class ResultExtensions
{
	/// <summary>
	/// Run <paramref name="bind"/> when <paramref name="this"/> is <see cref="Ok{T}"/>.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="bind">Function to convert a <typeparamref name="T"/> object to a <typeparamref name="TReturn"/> object.</param>
	/// <returns><see cref="Result{T}"/> object returned by <paramref name="bind"/> or <see cref="Err"/>.</returns>
	public static Result<TReturn> Bind<T, TReturn>(this Result<T> @this, Func<T, Result<TReturn>> bind) =>
		R.Match(@this,
			err: R.Err<TReturn>,
			ok: bind
		);

	/// <inheritdoc cref="Bind{T, TReturn}(Result{T}, Func{T, Result{TReturn}})"/>
	public static Task<Result<TReturn>> BindAsync<T, TReturn>(this Result<T> @this, Func<T, Task<Result<TReturn>>> bind) =>
		R.MatchAsync(@this,
			err: R.Err<TReturn>,
			ok: bind
		);

	/// <inheritdoc cref="Bind{T, TReturn}(Result{T}, Func{T, Result{TReturn}})"/>
	public static Task<Result<TReturn>> BindAsync<T, TReturn>(this Task<Result<T>> @this, Func<T, Result<TReturn>> bind) =>
		R.MatchAsync(@this,
			err: R.Err<TReturn>,
			ok: bind
		);

	/// <inheritdoc cref="Bind{T, TReturn}(Result{T}, Func{T, Result{TReturn}})"/>
	public static Task<Result<TReturn>> BindAsync<T, TReturn>(this Task<Result<T>> @this, Func<T, Task<Result<TReturn>>> bind) =>
		R.MatchAsync(@this,
			err: R.Err<TReturn>,
			ok: bind
		);
}
