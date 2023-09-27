// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// 'Error' Result - holds information about the error.
/// </summary>
public readonly struct Err : IUnion<ErrValue>
{
	/// <summary>
	/// Error Value object.
	/// </summary>
	public readonly required ErrValue Value { get; init; }
}
