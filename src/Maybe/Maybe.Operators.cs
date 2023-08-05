// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Monadic;

public abstract partial record class Maybe<T>
{
	public static implicit operator Maybe<T>(T value) =>
		M.Some(value);

	public static implicit operator Maybe<T>(Monadic.None _) =>
		None.Create();
}
