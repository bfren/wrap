// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <inheritdoc cref="IMonad{TMonad, TValue}"/>
/// <remarks>
/// This is the base type for all monad objects in the Wrap libraries,
/// though most implementations use the more useful <see cref="IMonad{T}"/>
/// and <see cref="IMonad{TMonad, TValue}"/>.
/// </remarks>
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
	/// Value - nullability will match that of <typeparamref name="T"/>.
	/// </summary>
	new T Value { get; init; }

	object IMonad.Value =>
		Value ?? new object();
}

/// <summary>
/// Single-case monad type to wrap an object value.
/// </summary>
/// <typeparam name="TMonad">Monad type.</typeparam>
/// <typeparam name="TValue">Value type.</typeparam>
public interface IMonad<TMonad, TValue> : IMonad<TValue>
	where TMonad : IMonad<TMonad, TValue>
{ }
