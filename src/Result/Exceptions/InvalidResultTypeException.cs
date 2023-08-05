// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Monadic.Exceptions;

public sealed class InvalidResultTypeException : InvalidTypeException
{
	internal InvalidResultTypeException(Type type) : base(type, typeof(Result<>), typeof(Err), typeof(Ok<>)) { }
}
