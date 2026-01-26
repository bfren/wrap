// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Testing;

/// <summary>
/// Generate StrongIds with random values.
/// </summary>
public static class FailureGenerator
{
	/// <summary>
	/// Creates a new instance of the <see cref="Failure"/> class with a randomly generated failure value.
	/// </summary>
	/// <remarks>Use this method to generate a failure result for testing or simulation purposes. The returned
	/// instance will have its value set to a randomly generated string.</remarks>
	/// <returns>A <see cref="Failure"/> object initialized with a random failure value.</returns>
	public static Failure Create() =>
		new(new FailureValue(Rnd.Str));

	/// <summary>
	/// Creates a new instance of the <see cref="Failure"/> class with a randomly generated failure value.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="failure">[Optional] Provided Failvalue.</param>
	/// <returns>A <see cref="Failure"/> object implicitly returned as <see cref="Result{T}"/>.</returns>
	public static Result<T> Create<T>(FailureValue? failure = null) =>
		new Failure(failure ?? new(Rnd.Str));
}
