// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Monadic.Exceptions;

/// <summary>
/// Base exception thrown during Monadic operations.
/// </summary>
public abstract class MonadicException : Exception
{
	/// <summary>Create exception.</summary>
	public MonadicException() { }

	/// <summary>Create exception.</summary>
	/// <param name="message">Exception message.</param>
	public MonadicException(string message) : base(message) { }

	/// <summary>Create exception.</summary>
	/// <param name="message">Exception message.</param>
	/// <param name="inner">Inner exception.</param>
	public MonadicException(string message, Exception inner) : base(message, inner) { }
}
