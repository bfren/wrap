// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Monadic.Exceptions;

/// <summary>
/// Thrown when a switch function encounters an <see cref="Either{TLeft, TRight}"/> type that is
/// neither <see cref="Left{TLeft, TRight}"/> nor <see cref="Right{TLeft, TRight}"/>.
/// </summary>
public sealed class InvalidEitherTypeException : InvalidTypeException
{
	/// <summary>Create exception.</summary>
	/// <param name="type">The invalid type that has been encountered.</param>
	internal InvalidEitherTypeException(Type type) : base(type, typeof(Either<,>), typeof(Left<,>), typeof(Right<,>)) { }
}
