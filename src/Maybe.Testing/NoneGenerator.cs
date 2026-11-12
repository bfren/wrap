// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Testing;

/// <summary>
/// Generate <see cref="None"/> values typed as <see cref="Maybe{T}"/>.
/// </summary>
public static class NoneGenerator
{
	/// <summary>
	/// Create a <see cref="None"/> value as a <see cref="Maybe{T}"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <returns>A <see cref="None"/> object implicitly returned as <see cref="Maybe{T}"/>.</returns>
	public static Maybe<T> Create<T>() =>
		new None();
}
