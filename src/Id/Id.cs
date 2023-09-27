// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// ID monad.
/// </summary>
/// <typeparam name="TId">Implementation type.</typeparam>
/// <typeparam name="TValue">ID value type.</typeparam>
public abstract record class Id<TId, TValue> : Union<TId, TValue>
	where TId : Id<TId, TValue>, new()
	where TValue : struct
{
	/// <summary>
	/// Require ID implementations to provide a default value.
	/// </summary>
	/// <param name="value">Default ID value.</param>
	protected Id(TValue value) : base(value) { }
}
