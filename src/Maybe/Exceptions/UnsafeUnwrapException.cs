// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Exceptions;

/// <summary>
/// Thrown when <see cref="UnsafeExtensions.Unwrap{T}(Unsafe{T})"/> fails.
/// </summary>
public sealed class UnsafeUnwrapException : WrapException { }
