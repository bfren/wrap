// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Exceptions;

/// <summary>
/// Thrown when an attempt is made to Wrap null in a <see cref="Maybe{T}"/>.
/// </summary>
public sealed class NullMaybeValueException() : WrapException("Maybe value cannot be null.");
