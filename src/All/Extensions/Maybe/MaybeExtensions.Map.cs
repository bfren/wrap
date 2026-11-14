// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Convert the value with <paramref name="f"/> when <paramref name="this"/> is <see cref="Some{T}"/>, and wrap the result.
	/// </summary>
	/// <remarks>
	/// The result is wrapped with <see cref="M.Wrap{T}(T)"/>, so a <see langword="null"/> result becomes <see cref="None"/>.
	/// </remarks>
	/// <seealso cref="Bind{T, TReturn}(Maybe{T}, Func{T, Maybe{TReturn}})"/>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="f">Function that converts the value.</param>
	/// <returns><see cref="Some{T}"/> holding the value returned by <paramref name="f"/>, or <see cref="None"/> if <paramref name="this"/> is <see cref="None"/> or <paramref name="f"/> returns <see langword="null"/>.</returns>
	public static Maybe<TReturn> Map<T, TReturn>(this Maybe<T> @this, Func<T, TReturn> f) =>
		@this switch
		{
			Some<T> x =>
				M.Wrap(f(x.Value)),

			None =>
				M.None
		};

	/// <inheritdoc cref="Map{T, TReturn}(Maybe{T}, Func{T, TReturn})"/>
	public static async Task<Maybe<TReturn>> MapAsync<T, TReturn>(this Maybe<T> @this, Func<T, Task<TReturn>> f) =>
		@this switch
		{
			Some<T> x =>
				M.Wrap(await f(x.Value)),

			None =>
				M.None
		};

	/// <inheritdoc cref="Map{T, TReturn}(Maybe{T}, Func{T, TReturn})"/>
	public static async Task<Maybe<TReturn>> MapAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<T, TReturn> f) =>
		(await @this).Map(f);

	/// <inheritdoc cref="Map{T, TReturn}(Maybe{T}, Func{T, TReturn})"/>
	public static async Task<Maybe<TReturn>> MapAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<T, Task<TReturn>> f) =>
		await (await @this).MapAsync(f);
}
