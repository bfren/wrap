// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Implementation using <see cref="int"/> as the Value type
/// </summary>
/// <typeparam name="TId">ID implementation type.</typeparam>
public abstract record class IntId<TId> : Id<TId, int>
	where TId : IntId<TId>, new()
{
	/// <summary>
	/// Create ID with default value
	/// </summary>
	protected IntId() : base(0) { }

	/// <summary>
	/// Create ID with value
	/// </summary>
	/// <param name="value">ID Value</param>
	protected IntId(int value) : base(value) { }
}
