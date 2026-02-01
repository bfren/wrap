// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Exceptions;

/// <summary>
/// Thrown when <see cref="Union{TUnion, TValue}.Value"/> is accessed without being set.
/// </summary>
public sealed class NullUnionValueException() :
	WrapException("You must set the value of a Union type when creating it.");
