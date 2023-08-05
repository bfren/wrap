// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Monadic.Exceptions;

namespace Monadic;

/// <summary>
/// Pure functions for interacting with <see cref="Maybe{T}"/> types.
/// </summary>
public static partial class M
{
	public static InvalidMaybeTypeException Invalid<T>(Maybe<T> maybe) =>
		new(maybe.GetType());
}
