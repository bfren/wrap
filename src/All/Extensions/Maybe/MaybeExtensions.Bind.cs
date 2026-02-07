// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Run <paramref name="f"/> when <paramref name="this"/> is <see cref="Some{T}"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="f">Function to convert a <typeparamref name="T"/> object to a <typeparamref name="TReturn"/> object.</param>
	/// <returns><see cref="Maybe{T}"/> object returned by <paramref name="f"/> or <see cref="None"/>.</returns>
	public static Maybe<TReturn> Bind<T, TReturn>(this Maybe<T> @this, Func<T, Maybe<TReturn>> f) =>
		M.Match(@this,
			fNone: () => M.None,
			fSome: f
		);

	/// <inheritdoc cref="Bind{T, TReturn}(Maybe{T}, Func{T, Maybe{TReturn}})"/>
	public static Task<Maybe<TReturn>> BindAsync<T, TReturn>(this Maybe<T> @this, Func<T, Task<Maybe<TReturn>>> f) =>
		M.MatchAsync(@this,
			fNone: () => M.None,
			fSome: f
		);

	/// <inheritdoc cref="Bind{T, TReturn}(Maybe{T}, Func{T, Maybe{TReturn}})"/>
	public static Task<Maybe<TReturn>> BindAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<T, Maybe<TReturn>> f) =>
		M.MatchAsync(@this,
			fNone: () => M.None,
			fSome: f
		);

	/// <inheritdoc cref="Bind{T, TReturn}(Maybe{T}, Func{T, Maybe{TReturn}})"/>
	public static Task<Maybe<TReturn>> BindAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<T, Task<Maybe<TReturn>>> f) =>
		M.MatchAsync(@this,
			fNone: () => M.None,
			fSome: f
		);
}
