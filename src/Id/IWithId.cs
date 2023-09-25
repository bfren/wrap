// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Represents an object (Entity or Model) with a strongly-typed ID.
/// </summary>
/// <typeparam name="TId">ID type.</typeparam>
public interface IWithId<TId>
	where TId : Id<TId>, new()
{
	/// <summary>
	/// 
	/// </summary>
	TId Id { get; init; }
}
