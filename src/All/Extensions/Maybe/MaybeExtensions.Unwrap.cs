// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Return the value if <paramref name="this"/> is <see cref="Some{T}"/>, otherwise the value returned by <paramref name="fNone"/>.
	/// </summary>
	/// <seealso cref="Maybe{T}.Unwrap(Func{T})"/>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="fNone">Returns the value to use if <paramref name="this"/> is <see cref="None"/>.</param>
	/// <returns>The <see cref="Some{T}"/> value, or the value returned by <paramref name="fNone"/>.</returns>
	public static Task<T> UnwrapAsync<T>(this Maybe<T> @this, Func<Task<T>> fNone) =>
		M.MatchAsync(@this,
			fNone: fNone,
			fSome: x => x
		);

	/// <inheritdoc cref="UnwrapAsync{T}(Maybe{T}, Func{Task{T}})"/>
	public static Task<T> UnwrapAsync<T>(this Task<Maybe<T>> @this, Func<T> fNone) =>
		M.MatchAsync(@this,
			fNone: fNone,
			fSome: async x => x
		);

	/// <inheritdoc cref="UnwrapAsync{T}(Maybe{T}, Func{Task{T}})"/>
	public static Task<T> UnwrapAsync<T>(this Task<Maybe<T>> @this, Func<Task<T>> fNone) =>
		M.MatchAsync(@this,
			fNone: fNone,
			fSome: async x => x
		);
}
