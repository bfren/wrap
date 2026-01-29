// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap.Exceptions;

/// <summary>
/// Thrown when a switch function encounters a <see cref="Maybe{T}"/> type that is neither <see cref="Some{T}"/>
/// nor <see cref="Maybe{T}.NoneImpl"/>.
/// </summary>
public sealed class InvalidMaybeTypeException : InvalidTypeException
{
	/// <summary>Create exception.</summary>
	/// <param name="type">The invalid type that has been encountered.</param>
	internal InvalidMaybeTypeException(Type type) : base(type, typeof(Maybe<>), typeof(None), typeof(Some<>)) { }
}
