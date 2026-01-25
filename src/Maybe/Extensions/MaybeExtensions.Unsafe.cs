// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Wraps <paramref name="this"/> in an <see cref="Unsafe{T}(Maybe{T})"/> to enable unsafe functions.
	/// </summary>
	/// <param name="this">Maybe object.</param>
	/// <returns>Unsafe wrapper containing <paramref name="this"/>.</returns>
	public static Unsafe<T> Unsafe<T>(this Maybe<T> @this) =>
		new() { Maybe = @this };

	/// <inheritdoc cref="Unsafe{T}(Maybe{T})"/>
	public static async Task<Unsafe<T>> Unsafe<T>(this Task<Maybe<T>> @this) =>
		new() { Maybe = await @this };
}
