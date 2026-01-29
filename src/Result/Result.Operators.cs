// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

public abstract partial record class Result<T>
{
	/// <summary>
	/// Implicitly wrap <paramref name="value"/> as an <see cref="Ok{T}"/> object.
	/// </summary>
	/// <param name="value">Input value.</param>
	/// <returns>Wrapped value.</returns>
	public static implicit operator Result<T>(T value) =>
		R.Wrap(value);

	/// <summary>
	/// Implicitly convert a <see cref="Wrap.Failure"/> to a <see cref="Result{T}"/> failure.
	/// </summary>
	/// <param name="fail">Fail object.</param>
	/// <returns>Failure value.</returns>
	public static implicit operator Result<T>(Wrap.Failure fail) =>
		FailureImpl.Create(fail.Value);
}
