// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Exceptions;

/// <summary>
/// Thrown when <see cref="Monad{TMonad, TValue}.Value"/> is accessed without being set,
/// or an attempt is made to set it to null.
/// </summary>
public sealed class NullMonadValueException() : WrapException("Monad value cannot be null.");
