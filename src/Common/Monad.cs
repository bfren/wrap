// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Diagnostics.CodeAnalysis;
using Wrap.Exceptions;

namespace Wrap;

/// <see cref="IMonad{TMonad, TValue}"/>
public sealed record class Monad<T> : Monad<Monad<T>, T>;

/// <see cref="IMonad{TMonad, TValue}"/>
public abstract record class Monad<TMonad, TValue> : IMonad<TMonad, TValue>
	where TMonad : IMonad<TMonad, TValue>, new()
{
	/// <inheritdoc cref="IMonad{T}.Value"/>
	[MemberNotNull]
	public TValue Value
	{
		get => Check(field);
		init => field = Check(value);
	}

	/// <summary>
	/// Create an empty object.
	/// </summary>
	protected Monad() { }

	/// <summary>
	/// Allow base classes to set <see cref="Value"/> on construction.
	/// </summary>
	/// <param name="value">Monad value.</param>
	protected Monad([DisallowNull] TValue value) =>
		Value = value;

	/// <summary>
	/// Check <paramref name="value"/> and throw an exception if it is null and the underlying type
	/// does not allow null values.
	/// </summary>
	/// <param name="value">Value to check.</param>
	/// <returns><paramref name="value"/> if not null or null is permitted.</returns>
	/// <exception cref="NullMonadValueException">If <paramref name="value"/> is null and the underlying type is not nullable.</exception>
	private TValue Check(TValue? value) =>
		value switch
		{
			TValue =>
				value,

			_ when F.IsNullableValueType(value) =>
				value!,

			_ =>
				throw new NullMonadValueException()
		};

	/// <summary>
	/// Wrap a value as the current type.
	/// </summary>
	/// <param name="value">Monad value.</param>
	/// <returns>Wrapped <typeparamref name="TMonad"/> value.</returns>
#pragma warning disable CA1000 // Do not declare static members on generic types
	public static TMonad Wrap(TValue value) =>
		F.Wrap<TMonad, TValue>(value);
#pragma warning restore CA1000 // Do not declare static members on generic types
}
