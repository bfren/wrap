// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Implementation using <see cref="long"/> as the Value type
/// </summary>
public abstract record class LongId : Id<long>
{
	/// <summary>
	/// Create ID with default value
	/// </summary>
	protected LongId() : base(0L) { }

	/// <summary>
	/// Create ID with value
	/// </summary>
	/// <param name="value">ID Value</param>
	protected LongId(long value) : base(value) { }
}
