// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Base record for strongly typed IDs.
/// </summary>
/// <typeparam name="TId">ID implementation type.</typeparam>
/// <typeparam name="TValue">ID value type.</typeparam>
public abstract record class Id<TId, TValue> : Monad<TId, TValue>, IId<TId, TValue>
	where TId : Id<TId, TValue>, new()
	where TValue : struct
{
	/// <summary>
	/// Create the ID with a starting value, so every ID implementation has one.
	/// </summary>
	/// <param name="value">Starting ID value, usually the default for <typeparamref name="TValue"/>.</param>
	protected Id(TValue value) : base(value) { }
}
