// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Exceptions;

/// <summary>
/// Thrown when <see cref="Monad{TMonad, TValue}.Value"/> is accessed without being set.
/// </summary>
public sealed class NullMonadValueException() :
	WrapException("You must set the value of a Monad type when creating it.");
