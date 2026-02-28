// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Common utility functions for the Wrap library.
/// </summary>
public static partial class F
{
	/// <summary>
	/// Static exception logger - by default do nothing.
	/// </summary>
	public static ExceptionLogger? LogException { get; set; }

	/// <summary>
	/// Static failure logger - by default do nothing.
	/// </summary>
	public static FailureLogger? LogFailure { get; set; }
}
