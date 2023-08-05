// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Monadic.Exceptions;

public sealed class SimpleErrorException : Exception
{
	public SimpleErrorException() { }
	public SimpleErrorException(string message) : base(message) { }
	public SimpleErrorException(string message, Exception inner) : base(message, inner) { }
}
