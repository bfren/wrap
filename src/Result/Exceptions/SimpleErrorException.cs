// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap.Exceptions;

/// <summary>
/// Used to wrap a simple error message as an exception.
/// </summary>
/// <seealso cref="R.Err(string)"/>
/// <seealso cref="R.Err{T}(string)"/>
public sealed class SimpleErrorException : Exception
{
	/// <summary>Create exception.</summary>
	/// <param name="message">Error message.</param>
	internal SimpleErrorException(string message) : base(message) { }
}
