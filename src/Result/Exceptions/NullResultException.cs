// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap.Exceptions;

/// <summary>
/// Thrown when a null <see cref="Result{T}"/> object is encountered.
/// </summary>
public sealed class NullResultException : Exception
{
	/// <summary>Create exception.</summary>
	public NullResultException() { }
}
