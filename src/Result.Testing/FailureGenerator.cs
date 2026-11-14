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
	/// Create a <see cref="Failure"/> with a random message and two random arguments.
	/// </summary>
	/// <returns>A <see cref="Failure"/> with the <see cref="Logging.LogLevel.Warning"/> log level.</returns>
	public static Failure Create() =>
		new(Rnd.Str, Rnd.Int, Rnd.Guid);

	/// <summary>
	/// Create a <see cref="Failure"/> with a random message and two random arguments, as a <see cref="Result{T}"/>.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <returns>A <see cref="Failure"/> result.</returns>
	public static Result<T> Create<T>() =>
		Create();

	/// <summary>
	/// Create a <see cref="Failure"/> from <paramref name="ex"/>.
	/// </summary>
	/// <param name="ex">Exception.</param>
	/// <returns>A <see cref="Failure"/> with the exception's message and the <see cref="Logging.LogLevel.Error"/> log level.</returns>
	public static Failure Create(Exception ex) =>
		new(ex);

	/// <summary>
	/// Return <paramref name="value"/> as a <see cref="Result{T}"/>.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="value">Failure.</param>
	/// <returns>A <see cref="Failure"/> result.</returns>
	public static Result<T> Create<T>(Failure value) =>
		value;
}
