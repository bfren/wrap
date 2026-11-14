// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Run <paramref name="f"/> with the value when <paramref name="this"/> is <see cref="Some{T}"/>.
	/// </summary>
	/// <seealso cref="Map{T, TReturn}(Maybe{T}, Func{T, TReturn})"/>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="f">Function that converts the value to a <see cref="Maybe{T}"/>.</param>
	/// <returns>The <see cref="Maybe{T}"/> returned by <paramref name="f"/>, or <see cref="None"/> if <paramref name="this"/> is <see cref="None"/>.</returns>
	public static Maybe<TReturn> Bind<T, TReturn>(this Maybe<T> @this, Func<T, Maybe<TReturn>> f) =>
		@this switch
		{
			Some<T> x =>
				f(x.Value),

			None =>
				M.None
		};

	/// <inheritdoc cref="Bind{T, TReturn}(Maybe{T}, Func{T, Maybe{TReturn}})"/>
	public static async Task<Maybe<TReturn>> BindAsync<T, TReturn>(this Maybe<T> @this, Func<T, Task<Maybe<TReturn>>> f) =>
		@this switch
		{
			Some<T> x =>
				await f(x.Value),

			None =>
				M.None
		};

	/// <inheritdoc cref="Bind{T, TReturn}(Maybe{T}, Func{T, Maybe{TReturn}})"/>
	public static async Task<Maybe<TReturn>> BindAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<T, Maybe<TReturn>> f) =>
		(await @this).Bind(f);

	/// <inheritdoc cref="Bind{T, TReturn}(Maybe{T}, Func{T, Maybe{TReturn}})"/>
	public static async Task<Maybe<TReturn>> BindAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<T, Task<Maybe<TReturn>>> f) =>
		await (await @this).BindAsync(f);
}
