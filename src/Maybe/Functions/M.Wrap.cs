// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

public static partial class M
{
	/// <summary>
	/// One of the most important functions in the library: takes a value and returns
	/// either <see cref="Some{T}"/> or <see cref="Wrap.None"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// If <paramref name="value"/> is not null, you will get a <see cref="Some{T}"/> object where
	/// <see cref="Some{T}.Value"/> is <paramref name="value"/>.
	/// </para>
	/// <para>
	/// If <paramref name="value"/> is null, you will get a <see cref="Wrap.None"/> object.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="value">Value to wrap.</param>
	/// <returns>
	/// <see cref="Some{T}"/> if <paramref name="value"/> is not null - otherwise <see cref="Wrap.None"/>.
	/// </returns>
	public static Maybe<T> Wrap<T>(T value) =>
		value switch
		{
			T =>
				new Some<T>(value),

			_ =>
				None
		};
}
