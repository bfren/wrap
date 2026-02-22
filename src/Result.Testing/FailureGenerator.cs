// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap.Testing;

/// <summary>
/// Generate Failures with random values.
/// </summary>
public static class FailureGenerator
{
	/// <summary>
	/// Create a randomly generated failure value.
	/// </summary>
	public static FailureValue Value =>
		new(Rnd.Str, Rnd.Int, Rnd.Guid);

	/// <summary>
	/// Creates a new instance of the <see cref="Failure"/> class with a randomly generated failure value.
	/// </summary>
	/// <remarks>Use this method to generate a failure result for testing or simulation purposes. The returned
	/// instance will have its value set to a randomly generated string.</remarks>
	/// <returns>A <see cref="Failure"/> object initialized with a random failure value.</returns>
	public static Failure Create() =>
		new(Value);

	/// <summary>
	/// Creates a new instance of the <see cref="Failure"/> class with a randomly generated failure value.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <returns>A <see cref="Failure"/> object implicitly returned as <see cref="Result{T}"/>.</returns>
	public static Result<T> Create<T>() =>
		new Failure(Value);

	/// <summary>
	/// Creates a new instance of the <see cref="Failure"/> class with a provided Exception.
	/// </summary>
	/// <remarks>Use this method to generate a failure result for testing or simulation purposes. The returned
	/// instance will have its value set to a randomly generated string.</remarks>
	/// <param name="ex">Exception.</param>
	/// <returns>A <see cref="Failure"/> object initialized with a random failure value.</returns>
	public static Failure Create(Exception ex) =>
		new(ex);

	/// <summary>
	/// Creates a new instance of the <see cref="Failure"/> class with the specified failure value.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="value">Provided FailureValue.</param>
	/// <returns>A <see cref="Failure"/> object implicitly returned as <see cref="Result{T}"/>.</returns>
	public static Result<T> Create<T>(FailureValue value) =>
		new Failure(value);
}
