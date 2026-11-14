// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap.Exceptions;

/// <summary>
/// Base class for exceptions thrown by the Wrap libraries.
/// </summary>
public abstract class WrapException : Exception
{
	/// <summary>Create the exception with the default message.</summary>
	protected WrapException() { }

	/// <summary>Create the exception with a message.</summary>
	/// <param name="message">Exception message.</param>
	protected WrapException(string message) : base(message) { }

	/// <summary>Create the exception with a message and the exception that caused it.</summary>
	/// <param name="message">Exception message.</param>
	/// <param name="inner">Exception that caused this one.</param>
	protected WrapException(string message, Exception inner) : base(message, inner) { }
}
