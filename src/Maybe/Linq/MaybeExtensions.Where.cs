// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;
using Wrap.Extensions;

namespace Wrap.Linq;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Enables LINQ Where() on <see cref="Maybe{T}"/> objects.
	/// </summary>
	/// <remarks>
	/// <para>
	/// For example:
	/// </para>
	/// <code>
	/// from x in Maybe
	/// where x == y
	/// select x
	/// </code>
	/// <para>
	/// Returns value of x if <see cref="Maybe{T}"/> object is <see cref="Some{T}"/> and x is equal to y,
	/// and <see cref="None"/> if not.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">Maybe type</typeparam>
	/// <param name="this">Maybe</param>
	/// <param name="predicate">Select where predicate</param>
	public static Maybe<T> Where<T>(this Maybe<T> @this, Func<T, bool> predicate) =>
		@this.Filter(predicate);

	/// <inheritdoc cref="Where{T}(Maybe{T}, Func{T, bool})"/>
	public static Task<Maybe<T>> Where<T>(this Maybe<T> @this, Func<T, Task<bool>> predicate) =>
		@this.FilterAsync(predicate);

	/// <inheritdoc cref="Where{T}(Maybe{T}, Func{T, bool})"/>
	public static Task<Maybe<T>> Where<T>(this Task<Maybe<T>> @this, Func<T, bool> predicate) =>
		@this.FilterAsync(predicate);

	/// <inheritdoc cref="Where{T}(Maybe{T}, Func{T, bool})"/>
	public static Task<Maybe<T>> Where<T>(this Task<Maybe<T>> @this, Func<T, Task<bool>> predicate) =>
		@this.FilterAsync(predicate);
}
