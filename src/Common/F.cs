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
	/// <param name="args">[Optional] Arguments to use when <paramref name="failure"/> contains placeholders.</param>
	public delegate void FailureLogger(string failure, object? args = null);

	/// <summary>
	/// Log exceptions.
	/// </summary>
	/// <param name="exception">Exception object.</param>
	public delegate void ExceptionLogger(Exception exception);
}
