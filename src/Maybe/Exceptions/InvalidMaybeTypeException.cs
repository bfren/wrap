// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Monadic.Exceptions;

public sealed class InvalidMaybeTypeException : InvalidTypeException
{
	internal InvalidMaybeTypeException(Type type) : base(type, typeof(Maybe<>), typeof(None), typeof(Some<>)) { }
}
