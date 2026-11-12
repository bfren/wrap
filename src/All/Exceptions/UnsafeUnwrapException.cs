// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap.Exceptions;

public sealed class UnsafeUnwrapException : Exception
{
	public UnsafeUnwrapException(object? left, Exception? inner) : base($"Unsafe Unwrap threw an execption: {left}", inner) { }
}
