// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Monadic.Exceptions;

public abstract class InvalidTypeException : MonadicException
{
	protected InvalidTypeException(Type type, Type either, Type left, Type right) : base($"{type} is not a valid {either} - use {left} or {right} instead.") { }
}
