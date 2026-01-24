// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Represents an object (Entity or Model) with a strongly-typed ID.
/// </summary>
public interface IWithId
{
	/// <summary>
	/// ID value.
	/// </summary>
	IUnion Id { get; }
}

/// <summary>
/// Represents an object (Entity or Model) with a strongly-typed ID.
/// </summary>
/// <typeparam name="T">ID Value type.</typeparam>
public interface IWithId<T> : IWithId
{
	/// <inheritdoc cref="IWithId.Id"/>
	new IUnion<T> Id { get; }

	/// <inheritdoc/>
	IUnion IWithId.Id =>
		Id;
}

/// <summary>
/// Represents an object (Entity or Model) with a strongly-typed ID.
/// </summary>
/// <typeparam name="TId">ID type.</typeparam>
/// <typeparam name="TValue">ID Value type.</typeparam>
public interface IWithId<TId, TValue> : IWithId<TValue>
	where TId : IId<TId, TValue>, new()
	where TValue : struct
{
	/// <inheritdoc cref="IWithId.Id"/>
	new TId Id { get; init; }

	/// <inheritdoc/>
	IUnion<TValue> IWithId<TValue>.Id =>
		Id;
}
