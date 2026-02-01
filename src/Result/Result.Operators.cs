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
	/// Implicitly convert a <see cref="Failure"/> to a <see cref="Result{T}"/> failure.
	/// </summary>
	/// <param name="fail">Failure object.</param>
	/// <returns>Failure value.</returns>
	public static implicit operator Result<T>(Failure fail) =>
		new FailureImpl(fail.Value);

	/// <summary>
	/// Implicitly convert a <see cref="Union{T}"/> into a <see cref="Result{T}"/> object.
	/// </summary>
	/// <param name="obj">Wrapped object.</param>
	public static implicit operator Result<T>(Union<T> obj) =>
		R.Wrap(obj.Value);
}
