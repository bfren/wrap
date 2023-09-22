// Monads: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Monads;

public readonly struct Err
{
	public readonly required ErrValue Value { get; init; }
}
