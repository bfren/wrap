// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

public abstract partial record class Result<T>
{
	/// <summary>
	/// Wrap <paramref name="value"/> as a <see cref="Ok{T}"/> object.
	/// </summary>
	/// <param name="value">Input value.</param>
	public static implicit operator Result<T>(T value) =>
		R.Wrap(value);

	/// <summary>
	/// Implicitly convert a <see cref="Wrap.Err"/> into a <see cref="Result{T}.Err"/> object.
	/// </summary>
	/// <param name="err">Error value.</param>
	public static implicit operator Result<T>(Wrap.Err err) =>
		Err.Create(err.Value);
}
