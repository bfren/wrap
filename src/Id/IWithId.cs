// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Object (entity or model) with a strongly typed ID.
/// </summary>
public interface IWithId
{
	/// <summary>
	/// Gets the ID as an untyped monad.
	/// </summary>
	IMonad Id { get; }
}

/// <summary>
/// Object (entity or model) with a strongly typed ID of type <typeparamref name="TId"/>.
/// </summary>
/// <typeparam name="TId">ID type.</typeparam>
/// <typeparam name="TValue">ID value type.</typeparam>
public interface IWithId<TId, TValue> : IWithId
	where TId : class, IId<TId, TValue>, new()
	where TValue : struct
{
	/// <summary>
	/// Gets or initialises the strongly typed ID.
	/// </summary>
	new TId Id { get; init; }

	IMonad IWithId.Id =>
		Id;
}
