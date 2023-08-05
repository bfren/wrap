// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Monadic.Exceptions;

public abstract class MonadicException : Exception
{
	public MonadicException() { }
	public MonadicException(string message) : base(message) { }
	public MonadicException(string message, Exception inner) : base(message, inner) { }
}
