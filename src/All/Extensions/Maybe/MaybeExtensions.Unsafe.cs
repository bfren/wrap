// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Wrap <paramref name="this"/> in an <see cref="Unsafe{TEither, TLeft, TRight}"/> object, to use the unsafe functions.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <returns>An <see cref="Unsafe{TEither, TLeft, TRight}"/> object wrapping <paramref name="this"/>.</returns>
	public static Unsafe<Maybe<T>, None, T> Unsafe<T>(this Maybe<T> @this) =>
		new(@this);

	/// <inheritdoc cref="Unsafe{T}(Maybe{T})"/>
	public static async Task<Unsafe<Maybe<T>, None, T>> Unsafe<T>(this Task<Maybe<T>> @this) =>
		new() { Value = await @this };
}
