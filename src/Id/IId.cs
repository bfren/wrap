// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// ID monad.
/// </summary>
/// <typeparam name="TValue">ID value type.</typeparam>
public interface IId<TValue> : IUnion<TValue>;

/// <summary>
/// ID monad.
/// </summary>
/// <typeparam name="TId">ID implementation type.</typeparam>
/// <typeparam name="TValue">ID value type.</typeparam>
public interface IId<TId, TValue> : IId<TValue>
	where TId : IId<TId, TValue>, new()
	where TValue : struct
{
	/// <summary>
	/// ID Value.
	/// </summary>
	new TValue Value { get; init; }
}
