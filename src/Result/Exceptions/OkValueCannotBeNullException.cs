// Monads: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Monads.Exceptions;

/// <summary>
/// An <see cref="Ok{T}"/> object vannot receive a null value.
/// </summary>
/// <seealso cref="R.Wrap{T}(T)"/>
public sealed class OkValueCannotBeNullException : Exception
{
	/// <summary>Create exception.</summary>
	public OkValueCannotBeNullException() { }
}
