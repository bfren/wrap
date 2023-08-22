// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Monadic.Exceptions;

/// <summary>
/// An <see cref="Ok{T}"/> object vannot receive a null value.
/// </summary>
/// <seealso cref="R.Ok{T}(T)"/>
public sealed class OkValueCannotBeNullException : Exception
{
	/// <inheritdoc cref="OkValueCannotBeNullException(string, Exception)"/>
	public OkValueCannotBeNullException() { }

	/// <inheritdoc cref="OkValueCannotBeNullException(string, Exception)"/>
	public OkValueCannotBeNullException(string message) : base(message) { }

	/// <summary>Create exception.</summary>
	/// <param name="message">Error message.</param>
	/// <param name="inner">Inner exception.</param>
	public OkValueCannotBeNullException(string message, Exception inner) : base(message, inner) { }
}
