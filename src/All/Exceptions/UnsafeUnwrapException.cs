// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap.Exceptions;

public sealed class UnsafeUnwrapException(object? left, Exception? inner) :
	Exception($"Unsafe Unwrap threw an exception: {left}", inner);
