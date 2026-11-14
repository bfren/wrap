// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Exceptions;

/// <summary>
/// Thrown when <see cref="Monad{TMonad, TValue}.Value"/> is read before it is set, or when
/// <see langword="null"/> is wrapped in a monad whose value type is not a nullable value type.
/// </summary>
public sealed class NullMonadValueException() : WrapException("Monad value cannot be null.");
