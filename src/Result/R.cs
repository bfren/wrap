// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

/// <summary>
/// Pure functions for creating and matching <see cref="Result{T}"/> objects.
/// </summary>
public static partial class R
{
	/// <summary>
	/// Create a <see cref="Failure"/> from a message, for custom error handling when an operation fails.
	/// </summary>
	/// <param name="message">Failure message.</param>
	/// <param name="args">[Optional] Arguments to use when <paramref name="message"/> contains placeholders.</param>
	/// <returns>Failure.</returns>
	public delegate Failure ErrorHandler(string message, params object?[] args);

	/// <summary>
	/// Create a <see cref="Failure"/> from an exception thrown by an operation - see <see cref="Try{T}(Func{T}, ExceptionHandler)"/>.
	/// </summary>
	/// <param name="e">Exception object.</param>
	/// <returns>Failure.</returns>
	public delegate Failure ExceptionHandler(Exception e);

	/// <summary>
	/// Gets the default exception handler, which creates a <see cref="Failure"/> from the exception's message
	/// with the <see cref="Logging.LogLevel.Error"/> log level.
	/// </summary>
	public static ExceptionHandler DefaultExceptionHandler =>
		Fail;
}
