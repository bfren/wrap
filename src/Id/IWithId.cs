// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Represents an object (Entity or Model) with a strongly-typed ID.
/// </summary>
public interface IWithId
{
	/// <summary>
	/// Generic ID value.
	/// </summary>
	IUnion Id { get; }
}

/// <summary>
/// Represents an object (Entity or Model) with a strongly-typed ID.
/// </summary>
public interface IWithId<TId, TValue> : IWithId
	where TId : IId<TId, TValue>, new()
	where TValue : struct
{
	/// <summary>
	/// Generic ID value.
	/// </summary>
	new TId Id { get; }

	/// <inheritdoc/>
	IUnion IWithId.Id =>
		Id;
}
