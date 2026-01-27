// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Exceptions;

namespace Wrap;

public static partial class F
{
	/// <summary>
	/// Wrap <paramref name="value"/> into type <typeparamref name="TUnion"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// If <paramref name="value"/> is null and <typeparamref name="TValue"/> is not nullable and has a
	/// <see langword="null"/> default value, a <see cref="NullUnionValueException"/> will be thrown.
	/// </para>
	/// </remarks>
	/// <typeparam name="TUnion">Union type.</typeparam>
	/// <typeparam name="TValue">Union value type.</typeparam>
	/// <param name="value">Value to wrap.</param>
	/// <returns>Value wrapped as <typeparamref name="TUnion"/>.</returns>
	/// <exception cref="NullUnionValueException"></exception>
	public static TUnion Wrap<TUnion, TValue>(TValue? value)
		where TUnion : IUnion<TValue>, new() =>
		value switch
		{
			TValue =>
				new() { Value = value },

			_ when default(TValue) is not null =>
				new() { Value = default! },

			_ when IsNullableValueType(value) =>
				new() { Value = value! },

			_ =>
				throw new NullUnionValueException()
		};
}
