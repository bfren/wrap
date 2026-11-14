// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Keep the value of <paramref name="this"/> only if <paramref name="fTest"/> returns <see langword="true"/>.
	/// </summary>
	/// <seealso cref="Filter{T}(Maybe{T}, Func{T, bool})"/>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="fTest">Test to run on the value.</param>
	/// <returns><paramref name="this"/> if it is <see cref="Some{T}"/> and <paramref name="fTest"/> returns <see langword="true"/>, otherwise <see cref="None"/>.</returns>
	public static Maybe<T> ContinueIf<T>(this Maybe<T> @this, Func<T, bool> fTest) =>
		If<T, T>(@this, fTest, x => x, _ => M.None);

	/// <inheritdoc cref="ContinueIf{T}(Maybe{T}, Func{T, bool})"/>
	public static Task<Maybe<T>> ContinueIfAsync<T>(this Task<Maybe<T>> @this, Func<T, bool> fTest) =>
		IfAsync<T, T>(@this, fTest, x => x, _ => M.None);

}
