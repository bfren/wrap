// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

/// <summary>
/// Pure functions for interacting with Monad types.
/// </summary>
public static partial class F
{
	/// <summary>
	/// Log failures.
	/// </summary>
	/// <param name="failure">Failure message.</param>
	public delegate void FailureLogger(string failure);

	/// <summary>
	/// Log exceptions.
	/// </summary>
	/// <param name="exception">Exception object.</param>
	public delegate void ExceptionLogger(Exception exception);
}
