// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Monadic;

public sealed record class Err
{
	public Exception Value { get; private init; }

	internal Err(Exception value) =>
		Value = value;
}
