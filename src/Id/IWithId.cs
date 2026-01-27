// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Represents an object (Entity or Model) with a strongly-typed ID.
/// </summary>
public interface IWithId
{
	/// <summary>
	/// Union ID value.
	/// </summary>
	IUnion Id { get; }
}

/// <summary>
/// Represents an object (Entity or Model) with a strongly-typed ID.
/// </summary>
/// <typeparam name="T">ID Value type.</typeparam>
public interface IWithId<T> : IWithId
{
	/// <summary>
	/// Union ID value with specific type.
	/// </summary>
	new IUnion<T> Id { get; }
}

/// <summary>
/// Represents an object (Entity or Model) with a strongly-typed ID.
/// </summary>
/// <typeparam name="TId">ID type.</typeparam>
/// <typeparam name="TValue">ID Value type.</typeparam>
public interface IWithId<TId, TValue> : IWithId<TValue>
	where TId : class, IId<TId, TValue>, new()
	where TValue : struct
{
	/// <summary>
	/// Strongly-typed ID value.
	/// </summary>
	new TId Id { get; init; }
}
