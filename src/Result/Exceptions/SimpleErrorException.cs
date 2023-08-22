// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Monadic.Exceptions;

/// <summary>
/// Used to wrap a simple error message as an exception.
/// </summary>
/// <seealso cref="R.Err(string)"/>
/// <seealso cref="R.Err{T}(string)"/>
public sealed class SimpleErrorException : Exception
{
	/// <inheritdoc cref="SimpleErrorException(string, Exception)"/>
	public SimpleErrorException() { }

	/// <inheritdoc cref="SimpleErrorException(string, Exception)"/>
	public SimpleErrorException(string message) : base(message) { }

	/// <summary>Create exception.</summary>
	/// <param name="message">Error message.</param>
	/// <param name="inner">Inner exception.</param>
	public SimpleErrorException(string message, Exception inner) : base(message, inner) { }
}
