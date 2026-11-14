// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap.Exceptions;

/// <summary>
/// The exception thrown by <c>Unsafe().UnwrapSingle()</c> when the list is empty or has more than one item.
/// </summary>
/// <typeparam name="TRight">List type.</typeparam>
/// <typeparam name="TSingle">List item type.</typeparam>
public sealed class UnsafeUnwrapSingleException<TRight, TSingle>() :
	Exception($"'{typeof(TRight)}' is not a single-value collection of type '{typeof(TSingle)}'.");
