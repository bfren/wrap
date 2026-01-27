// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;
using Wrap.Extensions;

namespace Wrap.Linq;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Enables LINQ Select() on <see cref="Maybe{T}"/> objects.
	/// </summary>
	/// <remarks>
	/// <para>
	/// For example:
	/// </para>
	/// <code>
	/// from x in Maybe
	/// select x
	/// </code>
	/// <para>
	/// Returns value of x if <see cref="Maybe{T}"/> object is <see cref="Some{T}"/>,
	/// and <see cref="None"/> if not.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">Maybe type</typeparam>
	/// <typeparam name="TReturn">Return type</typeparam>
	/// <param name="this">Maybe</param>
	/// <param name="f">Return map function</param>
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
