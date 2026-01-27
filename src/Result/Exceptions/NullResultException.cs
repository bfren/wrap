// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Exceptions;

/// <summary>
/// Thrown when a null <see cref="Result{T}"/> object is encountered.
/// </summary>
public sealed class NullResultException : WrapException { }
