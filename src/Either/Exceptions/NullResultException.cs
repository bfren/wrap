// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Exceptions;

/// <summary>
/// Thrown when a null <see cref="Either{TLeft, TRight}"/> object is encountered.
/// </summary>
public sealed class NullEitherException : WrapException { }
