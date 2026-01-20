// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Testing;

/// <summary>
/// Generate StrongIds with random values.
/// </summary>
public static class FailGenerator
{
	/// <summary>
	/// Creates a new instance of the <see cref="Fail"/> class with a randomly generated failure value.
	/// </summary>
	/// <remarks>Use this method to generate a failure result for testing or simulation purposes. The returned
	/// instance will have its value set to a randomly generated string.</remarks>
	/// <returns>A <see cref="Fail"/> object initialized with a random failure value.</returns>
	public static Fail Create() =>
		new() { Value = FailValue.Create(Rnd.Str) };
}
