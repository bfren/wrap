// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Exceptions;

namespace Wrap.Testing.Exceptions;

/// <summary>
/// Thrown when <see cref="ResultExtensions.UnsafeUnwrap{T}(Result{T})"/> fails.
/// </summary>
public sealed class UnsafeUnwrapException : WrapException { }
