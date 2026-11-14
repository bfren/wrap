// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;
using Wrap.Extensions;

namespace Wrap.Linq;

public static partial class MaybeToMaybeExtensions
{
	/// <summary>
	/// Convert the value of a <see cref="Maybe{T}"/> in a LINQ query - see <see cref="MaybeExtensions.Map{T, TReturn}(Maybe{T}, Func{T, TReturn})"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// For example:
	/// </para>
	/// <code>
	/// from x in maybe
	/// select x * 2
	/// </code>
	/// </remarks>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="f">Function that converts the value.</param>
	/// <returns><see cref="Some{T}"/> holding the value returned by <paramref name="f"/>, or <see cref="None"/> if <paramref name="this"/> is <see cref="None"/> or <paramref name="f"/> returns <see langword="null"/>.</returns>
	public static Maybe<TReturn> Select<T, TReturn>(this Maybe<T> @this, Func<T, TReturn> f) =>
		@this.Map(f);

	/// <inheritdoc cref="Select{T, TReturn}(Maybe{T}, Func{T, TReturn})"/>
	public static Task<Maybe<TReturn>> Select<T, TReturn>(this Maybe<T> @this, Func<T, Task<TReturn>> f) =>
		@this.MapAsync(f);

	/// <inheritdoc cref="Select{T, TReturn}(Maybe{T}, Func{T, TReturn})"/>
	public static Task<Maybe<TReturn>> Select<T, TReturn>(this Task<Maybe<T>> @this, Func<T, TReturn> f) =>
		@this.MapAsync(f);

	/// <inheritdoc cref="Select{T, TReturn}(Maybe{T}, Func{T, TReturn})"/>
	public static Task<Maybe<TReturn>> Select<T, TReturn>(this Task<Maybe<T>> @this, Func<T, Task<TReturn>> f) =>
		@this.MapAsync(f);
}
