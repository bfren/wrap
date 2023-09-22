// Wrap: .NET monads for functional style.
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
	/// Implicitly convert a <see cref="Wrap.None"/> into a <see cref="Maybe{T}.None"/> object.
	/// </summary>
	/// <param name="_"></param>
	public static implicit operator Maybe<T>(Wrap.None _) =>
		None.Create();
}
