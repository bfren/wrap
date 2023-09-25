// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <inheritdoc cref="IWithId"/>
/// <typeparam name="T">ID type.</typeparam>
public abstract record class WithId<T> : IWithId
	where T : class, IId, new()
{
	/// <summary>
	/// <see cref="IId"/> object of type <typeparamref name="T"/> wrapping an ID value.
	/// </summary>
	public required T Id { get; init; }

	IId IWithId.Id =>
		Id;
}
