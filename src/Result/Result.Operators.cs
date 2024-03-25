// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

public abstract partial record class Result<T>
{
	/// <summary>
	/// Implicitly wrap <paramref name="value"/> as an <see cref="Ok{T}"/> object.
	/// </summary>
	/// <param name="value">Input value.</param>
	public static implicit operator Result<T>(T value) =>
		R.Wrap(value);

	/// <summary>
	/// Implicitly convert a <see cref="Fail"/> into a <see cref="Result{T}.Failure"/> object.
	/// </summary>
	/// <param name="fail">Generic fail object.</param>
	public static implicit operator Result<T>(Fail fail) =>
		Failure.Create(fail.Value);
}
