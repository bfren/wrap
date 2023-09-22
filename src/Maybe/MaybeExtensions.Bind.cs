// Monads: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Monads;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Run <paramref name="bind"/> when <paramref name="this"/> is <see cref="Some{T}"/>.
	/// </summary>
	/// <typeparam name="T">Maybe value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="bind">Function to convert a <typeparamref name="T"/> object to a <typeparamref name="TReturn"/> object.</param>
	/// <returns><see cref="Maybe{T}"/> object returned by <paramref name="bind"/> or <see cref="None"/>.</returns>
	public static Maybe<TReturn> Bind<T, TReturn>(this Maybe<T> @this, Func<T, Maybe<TReturn>> bind) =>
		M.Switch(@this,
			none: M.None,
			some: bind
		);

	/// <inheritdoc cref="Bind{T, TReturn}(Maybe{T}, Func{T, Maybe{TReturn}})"/>
	public static Task<Maybe<TReturn>> BindAsync<T, TReturn>(this Maybe<T> @this, Func<T, Task<Maybe<TReturn>>> bind) =>
		M.SwitchAsync(@this,
			none: M.None,
			some: bind
		);

	/// <inheritdoc cref="Bind{T, TReturn}(Maybe{T}, Func{T, Maybe{TReturn}})"/>
	public static Task<Maybe<TReturn>> BindAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<T, Maybe<TReturn>> bind) =>
		M.SwitchAsync(@this,
			none: M.None,
			some: bind
		);

	/// <inheritdoc cref="Bind{T, TReturn}(Maybe{T}, Func{T, Maybe{TReturn}})"/>
	public static Task<Maybe<TReturn>> BindAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<T, Task<Maybe<TReturn>>> bind) =>
		M.SwitchAsync(@this,
			none: M.None,
			some: bind
		);
}
