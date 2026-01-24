// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <inheritdoc cref="IWithId"/>
public abstract record class WithId<TId, TValue> : IWithId<TId, TValue>
	where TId : Id<TId, TValue>, new()
	where TValue : struct
{
	/// <inheritdoc/>
	public TId Id { get; init; } = new();
}
