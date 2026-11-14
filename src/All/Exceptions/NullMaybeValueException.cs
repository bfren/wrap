// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Exceptions;

/// <summary>
/// Thrown by <see cref="Json.MaybeJsonConverter{T}"/> when the input has no JSON tokens, or its value reads as <see langword="null"/>.
/// </summary>
public sealed class NullMaybeValueException() : WrapException("Maybe value cannot be null.");
