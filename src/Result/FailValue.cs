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
	private const LogLevel DefaultMessageLevel = LogLevel.Verbose;

	private const LogLevel DefaultExceptionLevel = LogLevel.Error;

	/// <summary>
	/// [Optional] Arguments to use as values where <see cref="Message"/> contains format placeholders.
	/// </summary>
	public readonly object[]? Args { get; init; }

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

	/// <inheritdoc cref="Create{T}(string, object[])"/>
	public static FailValue Create(string message, params object[] args) =>
		new()
		{
			Message = message,
			Args = args,
			Level = DefaultMessageLevel
		};

	/// <summary>
	/// Create a <see cref="FailValue"/> object from a simple <paramref name="message"/>.
	/// </summary>
	/// <typeparam name="T">Failure context.</typeparam>
	/// <param name="message">Failure message.</param>
	/// <param name="args">[Optional] Arguments to use as values where <see cref="Message"/> contains format placeholders.</param>
	/// <returns>Object containing failure information.</returns>
	public static FailValue Create<T>(string message, params object[] args) =>
		new()
		{
			Context = typeof(T).FullName,
			Message = message,
			Args = args,
			Level = DefaultMessageLevel
		};

	/// <inheritdoc cref="Create{T}(Exception)"/>
	public static FailValue Create(Exception exception) =>
		new()
		{
			Message = exception.Message,
			Exception = exception,
			Level = DefaultExceptionLevel
		};

	/// <summary>
	/// Create a <see cref="FailValue"/> object from an <paramref name="exception"/>.
	/// </summary>
	/// <typeparam name="T">Failure context.</typeparam>
	/// <param name="exception">Failure exception.</param>
	/// <returns>Object containing failure information.</returns>
	public static FailValue Create<T>(Exception exception) =>
		new()
		{
			Context = typeof(T).FullName,
			Message = exception.Message,
			Exception = exception,
			Level = DefaultExceptionLevel
		};

	///// <summary>
	///// Convert a failure message to an <see cref="FailValue"/> object,
	///// setting <see cref="Level"/> to <see cref="LogLevel.Verbose"/>.
	///// </summary>
	///// <param name="message">Failure message.</param>
	//public static implicit operator FailValue(string message) =>
	//	Create(message);

	///// <summary>
	///// Convert an exception to an <see cref="FailValue"/> object
	///// setting <see cref="Level"/> to <see cref="LogLevel.Error"/>.
	///// </summary>
	///// <param name="exception">Exception object.</param>
	//public static implicit operator FailValue(Exception exception) =>
	//	Create(exception);
}
