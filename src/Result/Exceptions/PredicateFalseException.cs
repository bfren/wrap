// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Monadic.Exceptions;

public sealed class PredicateFalseException : Exception
{
	public PredicateFalseException() { }
	public PredicateFalseException(string message) : base(message) { }
	public PredicateFalseException(string message, Exception inner) : base(message, inner) { }
}
