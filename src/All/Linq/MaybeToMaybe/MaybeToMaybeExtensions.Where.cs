// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;
using Wrap.Extensions;

namespace Wrap.Linq;

public static partial class MaybeToMaybeExtensions
{
	/// <summary>
	/// Filter a <see cref="Maybe{T}"/> in a LINQ query - see <see cref="MaybeExtensions.Filter{T}(Maybe{T}, Func{T, bool})"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// For example:
	/// </para>
	/// <code>
	/// from x in maybe
	/// where x > 0
	/// select x
	/// </code>
	/// </remarks>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="fTest">Test to run on the value.</param>
	/// <returns><paramref name="this"/> if it is <see cref="Some{T}"/> and <paramref name="fTest"/> returns <see langword="true"/>, otherwise <see cref="None"/>.</returns>
	public static Maybe<T> Where<T>(this Maybe<T> @this, Func<T, bool> fTest) =>
		@this.Filter(fTest);

	/// <inheritdoc cref="Where{T}(Maybe{T}, Func{T, bool})"/>
	public static Task<Maybe<T>> Where<T>(this Maybe<T> @this, Func<T, Task<bool>> fTest) =>
		@this.FilterAsync(fTest);

	/// <inheritdoc cref="Where{T}(Maybe{T}, Func{T, bool})"/>
	public static Task<Maybe<T>> Where<T>(this Task<Maybe<T>> @this, Func<T, bool> fTest) =>
		@this.FilterAsync(fTest);

	/// <inheritdoc cref="Where{T}(Maybe{T}, Func{T, bool})"/>
	public static Task<Maybe<T>> Where<T>(this Task<Maybe<T>> @this, Func<T, Task<bool>> fTest) =>
		@this.FilterAsync(fTest);
}
