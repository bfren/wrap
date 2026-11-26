// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap.Exceptions;

/// <summary>
/// The exception thrown by <c>Unsafe().UnwrapSingle()</c> when the list is empty or has more than one item.
/// </summary>
/// <param name="rightType">List type.</param>
/// <param name="singleType">List item type.</param>
public sealed class UnsafeUnwrapSingleException(Type rightType, Type singleType) :
	WrapException($"'{rightType}' is not a single-value collection of type '{singleType}'.");
