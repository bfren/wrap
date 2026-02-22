// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap.Exceptions;

/// <summary>
/// Base exception thrown during Wrap operations.
/// </summary>
public abstract class WrapException : Exception
{
	/// <summary>Create exception.</summary>
	protected WrapException() { }

	/// <summary>Create exception.</summary>
	/// <param name="message">Exception message.</param>
	protected WrapException(string message) : base(message) { }

	/// <summary>Create exception.</summary>
	/// <param name="message">Exception message.</param>
	/// <param name="inner">Inner exception.</param>
	protected WrapException(string message, Exception inner) : base(message, inner) { }
}
