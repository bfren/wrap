// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

public abstract partial record class Maybe<T>
{
	/// <summary>
	/// Wrap <paramref name="value"/> as a <see cref="Maybe{T}"/> object.
	/// </summary>
	/// <param name="value">Input value.</param>
	public static implicit operator Maybe<T>(T value) =>
		M.Wrap(value);

	/// <summary>
	/// Implicitly convert a <see cref="None"/> into a <see cref="Maybe{T}.NoneImpl"/> object.
	/// </summary>
	/// <param name="_">None value (discarded).</param>
	public static implicit operator Maybe<T>(None _) =>
		new NoneImpl();

	/// <summary>
	/// Implicitly convert a <see cref="Union{T}"/> into a <see cref="Maybe{T}"/> object.
	/// </summary>
	/// <param name="obj">Wrapped object.</param>
	public static implicit operator Maybe<T>(Union<T> obj) =>
		M.Wrap(obj.Value);
}
