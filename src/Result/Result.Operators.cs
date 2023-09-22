// Monads: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Monads;

public abstract partial record class Result<T>
{
	public static implicit operator Result<T>(T value) =>
		R.Wrap(value);

	public static implicit operator Result<T>(Monads.Err err) =>
		Err.Create(err.Value);
}
