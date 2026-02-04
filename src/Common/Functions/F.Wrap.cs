// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Exceptions;

namespace Wrap;

public static partial class F
{
	/// <summary>
	/// Wrap <paramref name="value"/> into type <typeparamref name="TMonad"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// If <paramref name="value"/> is null and <typeparamref name="TValue"/> is not nullable and has a
	/// <see langword="null"/> default value, a <see cref="NullMonadValueException"/> will be thrown.
	/// </para>
	/// </remarks>
	/// <typeparam name="TMonad">Monad type.</typeparam>
	/// <typeparam name="TValue">Monad value type.</typeparam>
	/// <param name="value">Value to wrap.</param>
	/// <returns>Value wrapped as <typeparamref name="TMonad"/>.</returns>
	/// <exception cref="NullMonadValueException"></exception>
	public static TMonad Wrap<TMonad, TValue>(TValue? value)
		where TMonad : IMonad<TMonad, TValue>, new() =>
		value switch
		{
			TValue =>
				new() { Value = value },

			_ when default(TValue) is not null =>
				new() { Value = default! },

			_ when IsNullableValueType(value) =>
				new() { Value = value! },

			_ =>
				throw new NullMonadValueException()
		};

	/// <inheritdoc cref="Wrap{TMonad, TValue}(TValue)"/>
	public static TMonad Bind<TMonad, TValue>(TValue? value)
		where TMonad : IMonad<TMonad, TValue>, new() =>
		Wrap<TMonad, TValue>(value);
}
