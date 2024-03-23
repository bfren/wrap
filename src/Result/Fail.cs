// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// 'Fail' Result - holds information why an operation failed.
/// </summary>
public readonly struct Fail : IUnion<FailValue>
{
	/// <summary>
	/// Returns information about the failure.
	/// </summary>
	public readonly required FailValue Value { get; init; }
}
