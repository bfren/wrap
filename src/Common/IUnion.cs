// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <inheritdoc cref="IUnion{T}"/>
public interface IUnion
{
	/// <summary>
	/// Value.
	/// </summary>
	object Value { get; }
}

/// <summary>
/// Single-case Union monad.
/// </summary>
/// <typeparam name="T">Value type.</typeparam>
public interface IUnion<T> : IUnion
{
	/// <summary>
	/// Value - nullability will match the nullability of <typeparamref name="T"/>.
	/// </summary>
	new T Value { get; init; }

	object IUnion.Value =>
		Value ?? new object();
}

/// <summary>
/// Single-case Union monad.
/// </summary>
/// <typeparam name="TUnion">Union type.</typeparam>
/// <typeparam name="TValue">Value type.</typeparam>
public interface IUnion<TUnion, TValue> : IUnion<TValue>
	where TUnion : IUnion<TUnion, TValue>
{ }
