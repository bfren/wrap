// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using Wrap.Logging;

namespace Wrap.Exceptions;

/// <summary>
/// Usually used in constructors where returning a <see cref="Result{T}"/> object is not possible.
/// </summary>
/// <param name="failure">Failure value.</param>
public sealed class FailureException(FailureValue failure) : Exception(failure.Message, failure.Exception)
{
	/// <inheritdoc cref="FailureValue.Args"/>
	public object? Args { get; } =
		failure.Args;

	/// <summary>
	/// [Optional] Context (usually full type name) of the failure.
	/// </summary>
	public string? Context { get; } =
		failure.Context;

	/// <summary>
	/// Log level.
	/// </summary>
	public LogLevel Level { get; } =
		failure.Level;
}
