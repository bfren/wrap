// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using Wrap.Logging;

namespace Wrap;

/// <summary>
/// Holds information about an error - which may or may not have been caused by an exception.
/// </summary>
public readonly record struct FailValue
{

	/// <summary>
	/// [Optional] Exception object.
	/// </summary>
	public readonly Exception? Exception { get; init; }

	/// <summary>
	/// LogLevel - default value is <see cref="LogLevel.Unknown"/>.
	/// </summary>
	public readonly LogLevel Level { get; init; }

	/// <summary>
	/// Failure message.
	/// </summary>
	public readonly required string Message { get; init; }

	/// <summary>
	/// Convert a failure message to an <see cref="FailValue"/> object,
	/// setting <see cref="Level"/> to <see cref="LogLevel.Unknown"/>.
	/// </summary>
	/// <param name="message">Failure message.</param>
	public static implicit operator FailValue(string message) =>
		new() { Message = message, Level = LogLevel.Unknown };

	/// <summary>
	/// Convert an exception to an <see cref="FailValue"/> object
	/// setting <see cref="Level"/> to <see cref="LogLevel.Error"/>.
	/// </summary>
	/// <param name="exception">Exception object.</param>
	public static implicit operator FailValue(Exception exception) =>
		new() { Message = exception.Message, Exception = exception, Level = LogLevel.Error };
}
