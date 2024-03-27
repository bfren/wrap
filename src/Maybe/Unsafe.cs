// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Unsafe wrapper to enable unsafe (i.e. may result in null values) functions.
/// </summary>
/// <typeparam name="T">Some value type.</typeparam>
public readonly record struct Unsafe<T>
{
	/// <summary>
	/// Wrapped <see cref="Maybe{T}"/> object.
	/// </summary>
	internal Maybe<T> Maybe { get; init; }

	/// <summary>
	/// Internal creation only (via <see cref="MaybeExtensions.Unsafe{T}(Maybe{T})"/>).
	/// </summary>
	/// <param name="maybe"><see cref="Maybe{T}"/> object to wrap.</param>
	internal Unsafe(Maybe<T> maybe) =>
		Maybe = maybe;
}
