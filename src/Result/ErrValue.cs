// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

/// <summary>
/// Holds information about an error - which may or may not have been caused by an exception.
/// </summary>
public readonly record struct ErrValue
{
	/// <summary>
	/// Error message.
	/// </summary>
	public readonly required string Message { get; init; }

	/// <summary>
	/// [Optional] Exception object.
	/// </summary>
	public readonly Exception? Exception { get; init; }

	/// <summary>
	/// Convert an error message to an <see cref="ErrValue"/> object.
	/// </summary>
	/// <param name="message">Error message.</param>
	public static implicit operator ErrValue(string message) =>
		new() { Message = message };

	/// <summary>
	/// Convert an exception to an <see cref="ErrValue"/> opject.
	/// </summary>
	/// <param name="exception">Exception object.</param>
	public static implicit operator ErrValue(Exception exception) =>
		new() { Message = exception.Message, Exception = exception };
}
