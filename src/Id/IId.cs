// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Represents a strongly-typed ID - this should never be implemented directly - see
/// <see cref="GuidId"/>,
/// <see cref="IntId"/>,
/// <see cref="LongId"/>,
/// <see cref="UIntId"/>,
/// <see cref="ULongId"/>.
/// </summary>
/// <remarks>
/// This exists only to enable generic querying and parsing of values.
/// </remarks>
public interface IId
{
	/// <summary>
	/// ID Value.
	/// </summary>
	object Value { get; }
}
