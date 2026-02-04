// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <inheritdoc cref="IMonad{TMonad, TValue}"/>
public interface IMonad
{
	/// <summary>
	/// Value.
	/// </summary>
	object Value { get; }
}

/// <inheritdoc cref="IMonad{TMonad, TValue}"/>
public interface IMonad<T> : IMonad
{
	/// <summary>
	/// Value - nullability will match the nullability of <typeparamref name="T"/>.
	/// </summary>
	new T Value { get; init; }

	object IMonad.Value =>
		Value ?? new object();
}

/// <summary>
/// Single-case Monad monad to wrap an object.
/// </summary>
/// <typeparam name="TMonad">Monad type.</typeparam>
/// <typeparam name="TValue">Value type.</typeparam>
public interface IMonad<TMonad, TValue> : IMonad<TValue>
	where TMonad : IMonad<TMonad, TValue>
{ }
