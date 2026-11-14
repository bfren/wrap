// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Base record for objects (entities or models) with a strongly typed ID of type <typeparamref name="TId"/>.
/// </summary>
/// <typeparam name="TId">ID type.</typeparam>
/// <typeparam name="TValue">ID value type.</typeparam>
public abstract record class WithId<TId, TValue> : IWithId<TId, TValue>
	where TId : Id<TId, TValue>, new()
	where TValue : struct
{
	/// <summary>
	/// Gets or initialises the strongly typed ID; a new ID holds the default value.
	/// </summary>
	public TId Id { get; init; } = new();

	/// <inheritdoc/>
	IMonad IWithId.Id =>
		Id;

	/// <summary>
	/// Create the object with a new ID that holds the default value.
	/// </summary>
	protected WithId() { }

	/// <summary>
	/// Create the object with an existing ID.
	/// </summary>
	/// <param name="id">ID object.</param>
	protected WithId(TId id) : this() =>
		Id = id;

	/// <summary>
	/// Create the object with a new ID holding <paramref name="value"/>.
	/// </summary>
	/// <param name="value">ID value.</param>
	protected WithId(TValue value) : this() =>
		Id = new() { Value = value };
}
