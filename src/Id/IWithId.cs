// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Represents an object (Entity or Model) with a strongly-typed ID.
/// </summary>
/// <typeparam name="TId">ID type.</typeparam>
/// <typeparam name="TValue">ID value type.</typeparam>
public interface IWithId<TId, TValue>
	where TId : Id<TId, TValue>, new()
	where TValue : struct
{
	/// <summary>
	/// <typeparamref name="TId"/> object of type <typeparamref name="TValue"/> wrapping an ID value.
	/// </summary>
	TId Id { get; }
}
