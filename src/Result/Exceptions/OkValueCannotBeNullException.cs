// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Exceptions;

/// <summary>
/// An <see cref="Ok{T}"/> object vannot receive a null value.
/// </summary>
/// <seealso cref="R.Wrap{T}(T)"/>
public sealed class OkValueCannotBeNullException : WrapException { }
