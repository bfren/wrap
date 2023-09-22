// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap.Exceptions;

/// <summary>
/// Base exception thrown during Monad operations.
/// </summary>
public abstract class MonadException : Exception
{
	/// <summary>Create exception.</summary>
	public MonadException() { }

	/// <summary>Create exception.</summary>
	/// <param name="message">Exception message.</param>
	public MonadException(string message) : base(message) { }

	/// <summary>Create exception.</summary>
	/// <param name="message">Exception message.</param>
	/// <param name="inner">Inner exception.</param>
	public MonadException(string message, Exception inner) : base(message, inner) { }
}
