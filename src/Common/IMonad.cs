// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Monad type that wraps a value.
/// </summary>
/// <remarks>
/// This is the base type for all monad objects in the Wrap libraries,
/// though most implementations use the more useful <see cref="IMonad{T}"/>
/// and <see cref="IMonad{TMonad, TValue}"/>.
/// </remarks>
public interface IMonad
{
	/// <summary>
	/// Gets the wrapped value, boxed if it is a value type.
	/// </summary>
	object Value { get; }
}

/// <summary>
/// Monad type that wraps a value of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">Value type.</typeparam>
public interface IMonad<T> : IMonad
{
	/// <summary>
	/// Gets or initialises the wrapped value; its nullability matches that of <typeparamref name="T"/>.
	/// </summary>
	/// <remarks>
	/// Read through <see cref="IMonad"/>, a <see langword="null"/> value is returned as a new <see cref="object"/>.
	/// </remarks>
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
