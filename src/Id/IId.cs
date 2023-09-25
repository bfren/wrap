// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Represents a strongly-typed ID - this should never be implemented directly - see
/// <see cref="GuidId{TId}"/>,
/// <see cref="IntId{TId}"/>,
/// <see cref="LongId{TId}"/>,
/// <see cref="UIntId{TId}"/>,
/// <see cref="ULongId{TId}"/>.
/// </summary>
public interface IId
{
	/// <summary>
	/// ID Value.
	/// </summary>
	object Value { get; }
}
