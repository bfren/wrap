// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

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
