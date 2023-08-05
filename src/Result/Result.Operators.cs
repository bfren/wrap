// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Monadic;

public abstract partial record class Result<T>
{
	public static implicit operator Result<T>(T value) =>
		R.Ok(value);

	public static implicit operator Result<T>(Monadic.Err err) =>
		Err.Create(err.Value);
}
