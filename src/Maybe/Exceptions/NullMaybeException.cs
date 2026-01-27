// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Exceptions;

/// <summary>
/// Thrown when a null <see cref="Maybe{T}"/> object is encountered.
/// </summary>
public sealed class NullMaybeException : WrapException
{
	/// <summary>Create exception.</summary>
	internal NullMaybeException() { }
}
