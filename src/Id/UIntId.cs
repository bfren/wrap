// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Implementation using <see cref="uint"/> as the Value type
/// </summary>
/// <typeparam name="TId">ID implementation type.</typeparam>
public abstract record class UIntId<TId> : Id<TId, uint>
	where TId : UIntId<TId>, new()
{
	/// <summary>
	/// Create ID with default value
	/// </summary>
	protected UIntId() : base(0u) { }

	/// <summary>
	/// Create ID with value
	/// </summary>
	/// <param name="value">ID Value</param>
	protected UIntId(uint value) : base(value) { }
}
