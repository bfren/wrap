// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Return a value determined by the value of <paramref name="this"/> and the result of <paramref name="predicate"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="predicate">Uses value of <paramref name="this"/> to determines whether to pass on the original value or return <see cref="None"/>.</param>
	/// <returns>Original value or <see cref="None"/>.</returns>
	public static Maybe<T> ContinueIf<T>(this Maybe<T> @this, Func<T, bool> predicate) =>
		If<T, T>(@this, predicate, x => x, _ => M.None);

	/// <inheritdoc cref="ContinueIf{T}(Maybe{T}, Func{T, bool})"/>
	public static Task<Maybe<T>> ContinueIfAsync<T>(this Task<Maybe<T>> @this, Func<T, bool> predicate) =>
		IfAsync<T, T>(@this, predicate, x => x, _ => M.None);

}
