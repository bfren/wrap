// Monads: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Monads.Linq;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Enables LINQ SelectMany() on <see cref="Maybe{T}"/> objects.
	/// </summary>
	/// <remarks>
	/// <para>
	/// For example:
	/// </para>
	/// <code>
	/// from x in Maybe
	/// from y in Maybe
	/// select x + y
	/// </code>
	/// <para>
	/// Returns value of x + y if both <see cref="Maybe{T}"/> objects are <see cref="Some{T}"/>,
	/// and <see cref="None"/> if either is <see cref="None"/>.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">Maybe type</typeparam>
	/// <typeparam name="TInner">Interim type</typeparam>
	/// <typeparam name="TReturn">Return type</typeparam>
	/// <param name="this">Maybe</param>
	/// <param name="f">Interim bind function</param>
	/// <param name="g">Return map function</param>
	public static Maybe<TReturn> SelectMany<T, TInner, TReturn>(this Maybe<T> @this, Func<T, Maybe<TInner>> f, Func<T, TInner, TReturn> g) =>
		@this.Bind(x => from y in f(x) select g(x, y));

	/// <inheritdoc cref="SelectMany{T, TInner, TReturn}(Maybe{T}, Func{T, Maybe{TInner}}, Func{T, TInner, TReturn})"/>
	public static Task<Maybe<TReturn>> SelectMany<T, TInner, TReturn>(this Maybe<T> @this, Func<T, Task<Maybe<TInner>>> f, Func<T, TInner, TReturn> g) =>
		@this.BindAsync(x => from y in f(x) select g(x, y));

	/// <inheritdoc cref="SelectMany{T, TInner, TReturn}(Maybe{T}, Func{T, Maybe{TInner}}, Func{T, TInner, TReturn})"/>
	public static Task<Maybe<TReturn>> SelectMany<T, TInner, TReturn>(this Task<Maybe<T>> @this, Func<T, Maybe<TInner>> f, Func<T, TInner, TReturn> g) =>
		@this.BindAsync(x => from y in f(x) select g(x, y));

	/// <inheritdoc cref="SelectMany{T, TInner, TReturn}(Maybe{T}, Func{T, Maybe{TInner}}, Func{T, TInner, TReturn})"/>
	public static Task<Maybe<TReturn>> SelectMany<T, TInner, TReturn>(this Task<Maybe<T>> @this, Func<T, Task<Maybe<TInner>>> f, Func<T, TInner, TReturn> g) =>
		@this.BindAsync(x => from y in f(x) select g(x, y));
}
