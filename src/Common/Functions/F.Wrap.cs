// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Exceptions;

namespace Wrap;

public static partial class F
{
	/// <summary>
	/// Wrap <paramref name="value"/> into type <typeparamref name="TMonad"/>.
	/// </summary>
	/// <remarks>
	/// <see langword="null"/> is allowed only when <typeparamref name="TValue"/> is a nullable value type.
	/// </remarks>
	/// <typeparam name="TMonad">Monad type.</typeparam>
	/// <typeparam name="TValue">Monad value type.</typeparam>
	/// <param name="value">Value to wrap.</param>
	/// <returns>A new <typeparamref name="TMonad"/> holding <paramref name="value"/>.</returns>
	/// <exception cref="NullMonadValueException"><paramref name="value"/> is <see langword="null"/> and <typeparamref name="TValue"/> is not a nullable value type.</exception>
	public static TMonad Wrap<TMonad, TValue>(TValue? value)
		where TMonad : IMonad<TMonad, TValue>, new() =>
		value switch
		{
			TValue =>
				new() { Value = value },

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
