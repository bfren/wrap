// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using Wrap.Logging;

namespace Wrap;

/// <summary>
/// Holds information about a failure.
/// </summary>
public readonly record struct FailValue
{
	private const LogLevel DefaultFailureLevel = LogLevel.Warning;

	private const LogLevel DefaultExceptionLevel = LogLevel.Error;

	/// <summary>
	/// [Optional] Arguments to use as values where <see cref="Message"/> contains format placeholders.
	/// </summary>
	public readonly object?[] Args { get; init; }

	/// <summary>
	/// [Optional] Context (usually full type name) of the failure.
	/// </summary>
	public string? Context { get; init; }

	/// <summary>
	/// [Optional] Exception object.
	/// </summary>
	public readonly Exception? Exception { get; init; }

	/// <summary>
	/// Log level.
	/// </summary>
	public readonly required LogLevel Level { get; init; }

	/// <summary>
	/// Failure message.
	/// </summary>
	public readonly required string Message { get; init; }

	/// <summary>
	/// Create a <see cref="FailValue"/> from a simple failure message.
	/// </summary>
	/// <param name="message">Failure message.</param>
	/// <param name="args">[Optional] Arguments to use when <paramref name="message"/> contains placeholders.</param>
	/// <returns>FailValue.</returns>
	public static FailValue Create(string message, params object?[] args) =>
		new()
		{
			Message = message,
			Args = args,
			Level = DefaultFailureLevel
		};

	/// <summary>
	/// Create a <see cref="Fail"/> object from an exception.
	/// </summary>
	/// <param name="ex">Exception object.</param>
	/// <returns>FailValue.</returns>
	public static FailValue Create(Exception ex) =>
		new()
		{
			Message = ex.Message,
			Exception = ex,
			Level = DefaultExceptionLevel
		};
}
