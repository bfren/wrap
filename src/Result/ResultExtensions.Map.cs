// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap;

public static partial class ResultExtensions
{
	/// <summary>
	/// Run <paramref name="map"/> when <paramref name="this"/> is <see cref="Ok{T}"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This function maps a <typeparamref name="T"/> object to a <typeparamref name="TReturn"/> object,
	/// and wraps the result in <see cref="Ok{T}"/>.
	/// </para>
	/// </remarks>
	/// <seealso cref="Bind{T, TReturn}(Result{T}, Func{T, Result{TReturn}})"/>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="map">Function to convert a <typeparamref name="T"/> object to a <typeparamref name="TReturn"/> object.</param>
	/// <returns><see cref="Ok{T}"/> object or <see cref="Fail"/>.</returns>
	public static Result<TReturn> Map<T, TReturn>(this Result<T> @this, Func<T, TReturn> map) =>
		R.Match(@this,
			fail: R.Fail<TReturn>,
			ok: x => R.Try(() => map(x))
		);

	/// <inheritdoc cref="Map{T, TReturn}(Result{T}, Func{T, TReturn})"/>
	public static Task<Result<TReturn>> MapAsync<T, TReturn>(this Result<T> @this, Func<T, Task<TReturn>> map) =>
		R.MatchAsync(@this,
			fail: R.Fail<TReturn>,
			ok: x => R.TryAsync(() => map(x))
		);

	/// <inheritdoc cref="Map{T, TReturn}(Result{T}, Func{T, TReturn})"/>
	public static Task<Result<TReturn>> MapAsync<T, TReturn>(this Task<Result<T>> @this, Func<T, TReturn> map) =>
		R.MatchAsync(@this,
			fail: R.Fail<TReturn>,
			ok: x => R.Try(() => map(x))
		);

	/// <inheritdoc cref="Map{T, TReturn}(Result{T}, Func{T, TReturn})"/>
	public static Task<Result<TReturn>> MapAsync<T, TReturn>(this Task<Result<T>> @this, Func<T, Task<TReturn>> map) =>
		R.MatchAsync(@this,
			fail: R.Fail<TReturn>,
			ok: x => R.TryAsync(() => map(x))
		);
}
