// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Implementation using <see cref="ulong"/> as the Value type
/// </summary>
/// <typeparam name="TId">ID implementation type.</typeparam>
public abstract record class ULongId<TId> : Id<TId, ulong>
	where TId : ULongId<TId>, new()
{
	/// <summary>
	/// Create ID with default value
	/// </summary>
	protected ULongId() : base(0UL) { }

	/// <summary>
	/// Create ID with value
	/// </summary>
	/// <param name="value">ID Value</param>
	protected ULongId(ulong value) : base(value) { }
}
