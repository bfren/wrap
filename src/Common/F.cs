// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Globalization;

namespace Wrap;

/// <summary>
/// Pure functions for interacting with Monad types.
/// </summary>
public static partial class F
{
	/// <summary>
	/// Default culture (en-GB) - used when working with strings.
	/// </summary>
	public static CultureInfo DefaultCulture { get; set; } = CultureInfo.GetCultureInfo("en-GB");

	/// <summary>
	/// Log failures.
	/// </summary>
	/// <param name="failure">Failure message.</param>
	/// <param name="args">[Optional] Arguments to use when <paramref name="failure"/> contains placeholders.</param>
	public delegate void FailureLogger(string failure, params object?[] args);

	/// <summary>
	/// Log exceptions.
	/// </summary>
	/// <param name="exception">Exception object.</param>
	public delegate void ExceptionLogger(Exception exception);
}
