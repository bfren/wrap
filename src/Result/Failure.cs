// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// 'Failure' Result - holds information why an operation failed.
/// </summary>
public readonly struct Failure : IUnion<FailureValue>
{
	/// <summary>
	/// Returns information about the failure.
	/// </summary>
	public readonly required FailureValue Value { get; init; }
}
