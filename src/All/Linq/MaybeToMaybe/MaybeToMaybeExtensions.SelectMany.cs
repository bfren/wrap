// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Linq;

public static partial class MaybeToMaybeExtensions
{
	/// <summary>
	/// Combine two <see cref="Maybe{T}"/> objects in a LINQ query.
	/// </summary>
	/// <remarks>
	/// <para>
	/// For example:
	/// </para>
	/// <code>
	/// from x in maybe
	/// from y in otherMaybe
	/// select x + y
	/// </code>
	/// </remarks>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <typeparam name="TInner">Interim value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="f">Function that returns the interim <see cref="Maybe{T}"/>, given the value.</param>
	/// <param name="g">Function that combines the value and the interim value.</param>
	/// <returns><see cref="Some{T}"/> holding the value returned by <paramref name="g"/>, or <see cref="None"/> if <paramref name="this"/> or the interim value is <see cref="None"/>, or <paramref name="g"/> returns <see langword="null"/>.</returns>
	public static Maybe<TReturn> SelectMany<T, TInner, TReturn>(this Maybe<T> @this, Func<T, Maybe<TInner>> f, Func<T, TInner, TReturn> g) =>
		@this switch
		{
			Some<T> x when f(x.Value) is Some<TInner> y =>
				g(x.Value, y.Value),

			_ =>
				M.None
		};

	/// <inheritdoc cref="SelectMany{T, TInner, TReturn}(Maybe{T}, Func{T, Maybe{TInner}}, Func{T, TInner, TReturn})"/>
	public static async Task<Maybe<TReturn>> SelectMany<T, TInner, TReturn>(this Maybe<T> @this, Func<T, Task<Maybe<TInner>>> f, Func<T, TInner, TReturn> g) =>
		@this switch
		{
			Some<T> x when await f(x.Value) is Some<TInner> y =>
				g(x.Value, y.Value),

			_ =>
				M.None
		};

	/// <inheritdoc cref="SelectMany{T, TInner, TReturn}(Maybe{T}, Func{T, Maybe{TInner}}, Func{T, TInner, TReturn})"/>
	public static async Task<Maybe<TReturn>> SelectMany<T, TInner, TReturn>(this Task<Maybe<T>> @this, Func<T, Maybe<TInner>> f, Func<T, TInner, TReturn> g) =>
		await @this switch
		{
			Some<T> x when f(x.Value) is Some<TInner> y =>
				g(x.Value, y.Value),

			_ =>
				M.None
		};

	/// <inheritdoc cref="SelectMany{T, TInner, TReturn}(Maybe{T}, Func{T, Maybe{TInner}}, Func{T, TInner, TReturn})"/>
	public static async Task<Maybe<TReturn>> SelectMany<T, TInner, TReturn>(this Task<Maybe<T>> @this, Func<T, Task<Maybe<TInner>>> f, Func<T, TInner, TReturn> g) =>
		await @this switch
		{
			Some<T> x when await f(x.Value) is Some<TInner> y =>
				g(x.Value, y.Value),

			_ =>
				M.None
		};
}
