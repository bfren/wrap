// Monads: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Monads.Testing.Exceptions;

/// <summary>
/// Thrown when <see cref="MaybeExtensions.UnsafeUnwrap{T}(Maybe{T})"/> fails.
/// </summary>
public sealed class UnsafeUnwrapException : Exception
{
	/// <summary>Create exception.</summary>
	public UnsafeUnwrapException() { }

	/// <summary>Create exception.</summary>
	/// <param name="message">Exception message.</param>
	public UnsafeUnwrapException(string message) : base(message) { }

	/// <summary>Create exception.</summary>
	/// <param name="message">Exception message.</param>
	/// <param name="inner">Inner exception.</param>
	public UnsafeUnwrapException(string message, Exception inner) : base(message, inner) { }
}
