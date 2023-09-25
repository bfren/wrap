// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Represents an object (Entity or Model) with a strongly-typed ID.
/// </summary>
public interface IWithId
{
	/// <summary>
	/// <see cref="IId"/> object wrapping an ID value.
	/// </summary>
	IId Id { get; }
}
