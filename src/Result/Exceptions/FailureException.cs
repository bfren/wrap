// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap.Exceptions;

/// <summary>
/// Usually used in constructors where returning a <see cref="Result{T}"/> object is not possible.
/// </summary>
/// <param name="failure">Failure value.</param>
public sealed class FailureException(FailValue failure) : Exception(failure.ToString());
