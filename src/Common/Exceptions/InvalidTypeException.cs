// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap.Exceptions;

/// <summary>
/// Base exception thrown when an <see cref="IEither{TLeft, TRight}"/> monad is matched to an invalid type.
/// </summary>
public abstract class InvalidTypeException : WrapException
{
	/// <summary>
	/// Create exception.
	/// </summary>
	/// <param name="implementation">Implementation type.</param>
	/// <param name="either">Base <see cref="IEither{TLeft, TRight}"/> type.</param>
	/// <param name="left">Left value type.</param>
	/// <param name="right">Right value type.</param>
	protected InvalidTypeException(Type implementation, Type either, Type left, Type right) : base($"{implementation} is not a valid {either} - use {left} or {right}.") { }
}
