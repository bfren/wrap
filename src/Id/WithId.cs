// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <inheritdoc cref="IWithId"/>
public abstract record class WithId<TId, TValue> : IWithId<TId, TValue>
	where TId : Id<TId, TValue>, new()
	where TValue : struct
{
	/// <inheritdoc/>
	public TId Id { get; init; } = new();

	/// <inheritdoc/>
	IMonad IWithId.Id =>
		Id;

	/// <inheritdoc/>
	IMonad<TValue> IWithId<TValue>.Id =>
		Id;

	/// <summary>
	/// Create with blank ID.
	/// </summary>
	protected WithId() { }

	/// <summary>
	/// Create with pre-existing ID.
	/// </summary>
	/// <param name="id">ID object.</param>
	protected WithId(TId id) : this() =>
		Id = id;

	/// <summary>
	/// Create with pre-existing ID value.
	/// </summary>
	/// <param name="value">ID value.</param>
	protected WithId(TValue value) : this() =>
		Id = new() { Value = value };
}
