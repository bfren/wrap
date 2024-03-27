// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Globalization;
using System.Text;
using Wrap.Logging;

namespace Wrap;

/// <summary>
/// Holds information about a failure.
/// </summary>
public readonly record struct FailValue
{
#if NET8_0_OR_GREATER
	private static readonly CompositeFormat ContextFormat = CompositeFormat.Parse("{0}.{1}()");
#else
	private const string ContextFormat = "{0}.{1}()";
#endif

	private const LogLevel DefaultMessageLevel = LogLevel.Verbose;

	private const LogLevel DefaultExceptionLevel = LogLevel.Error;

	/// <summary>
	/// [Optional] Arguments to use as values where <see cref="Message"/> contains format placeholders.
	/// </summary>
	public readonly object?[]? Args { get; init; }

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

	/// <inheritdoc cref="Create(string, string, string, object[])"/>
	public static FailValue Create(string message, params object?[] args) =>
		new()
		{
			Message = message,
			Args = args,
			Level = DefaultMessageLevel
		};

	/// <summary>
	/// Create a <see cref="FailValue"/> object from a simple <paramref name="message"/>.
	/// </summary>
	/// <param name="class">Context class.</param>
	/// <param name="function">Context function.</param>
	/// <param name="message">Failure message.</param>
	/// <param name="args">[Optional] Arguments to use as values where <see cref="Message"/> contains format placeholders.</param>
	/// <returns>Object containing failure information.</returns>
	public static FailValue Create(string @class, string function, string message, params object?[] args) =>
		new()
		{
			Context = string.Format(CultureInfo.InvariantCulture, ContextFormat, @class, function),
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
	public static FailValue Create<T>(string message, params object?[] args) =>
		new()
		{
			Context = typeof(T).FullName,
			Message = message,
			Args = args,
			Level = DefaultMessageLevel
		};

	/// <inheritdoc cref="Create(string, string, Exception)"/>
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
	/// <param name="class">Context class.</param>
	/// <param name="function">Context function.</param>
	/// <param name="exception">Failure exception.</param>
	/// <returns>Object containing failure information.</returns>
	public static FailValue Create(string @class, string function, Exception exception) =>
		new()
		{
			Context = string.Format(CultureInfo.InvariantCulture, ContextFormat, @class, function),
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
}
