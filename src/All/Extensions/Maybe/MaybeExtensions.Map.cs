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
	/// <remarks>
	/// <para>
	/// This function maps a <typeparamref name="T"/> object to a <typeparamref name="TReturn"/> object,
	/// and wraps the result in <see cref="Some{T}"/>.
	/// </para>
	/// </remarks>
	/// <seealso cref="Bind{T, TReturn}(Maybe{T}, Func{T, Maybe{TReturn}})"/>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="f">Function to convert a <typeparamref name="T"/> object to a <typeparamref name="TReturn"/> object.</param>
	/// <returns><see cref="Some{T}"/> object or <see cref="None"/>.</returns>
	public static Maybe<TReturn> Map<T, TReturn>(this Maybe<T> @this, Func<T, TReturn> f) =>
		M.Match(@this,
			none: () => M.None,
			some: x => M.Wrap(f(x))
		);

	/// <inheritdoc cref="Map{T, TReturn}(Maybe{T}, Func{T, TReturn})"/>
	public static Task<Maybe<TReturn>> MapAsync<T, TReturn>(this Maybe<T> @this, Func<T, Task<TReturn>> f) =>
		M.MatchAsync(@this,
			none: () => M.None,
			some: async x => M.Wrap(await f(x))
		);

	/// <inheritdoc cref="Map{T, TReturn}(Maybe{T}, Func{T, TReturn})"/>
	public static Task<Maybe<TReturn>> MapAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<T, TReturn> f) =>
		M.MatchAsync(@this,
			none: () => M.None,
			some: x => M.Wrap(f(x))
		);

	/// <inheritdoc cref="Map{T, TReturn}(Maybe{T}, Func{T, TReturn})"/>
	public static Task<Maybe<TReturn>> MapAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<T, Task<TReturn>> f) =>
		M.MatchAsync(@this,
			none: () => M.None,
			some: async x => M.Wrap(await f(x))
		);
}
