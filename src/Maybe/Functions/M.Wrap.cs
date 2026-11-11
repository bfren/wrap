// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

public static partial class M
{
	/// <summary>
	/// One of the most important functions in the library: wrap a value as a <see cref="Maybe{T}"/>,
	/// turning <see langword="null"/> into <see cref="Wrap.None"/>.
	/// </summary>
	/// <remarks>
	/// This includes nullable value types: wrapping an <c>int?</c> that has no value gives <see cref="Wrap.None"/>.
	/// </remarks>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="value">Value to wrap.</param>
	/// <returns>
	/// <see cref="Some{T}"/> holding <paramref name="value"/> if it is not <see langword="null"/>, otherwise <see cref="Wrap.None"/>.
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
