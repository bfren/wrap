// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

public abstract partial record class Maybe<T>
{
	public int CompareTo(Maybe<T>? other) => throw new NotImplementedException();
}
