// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Monadic.Exceptions;

public sealed class InvalidEitherTypeException : InvalidTypeException
{
	private InvalidEitherTypeException(Type type) : base(type, typeof(Either<,>), typeof(Left<,>), typeof(Right<,>)) { }

	public static InvalidEitherTypeException Create(Type type) =>
		new(type);
}
