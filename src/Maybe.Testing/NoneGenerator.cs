// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Testing;

/// <summary>
/// Generate strongly-type None values.
/// </summary>
public static class NoneGenerator
{
	/// <summary>
	/// Returns a <see cref="None"/> value .
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <returns>A <see cref="None"/> object implicitly returned as <see cref="Maybe{T}"/>.</returns>
	public static Maybe<T> Create<T>() =>
		M.None;
}
