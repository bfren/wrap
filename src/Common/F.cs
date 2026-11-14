// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

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
}
