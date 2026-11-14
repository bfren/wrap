// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Globalization;

namespace Wrap;

/// <summary>
/// Pure functions for working with monad types.
/// </summary>
public static partial class F
{
	/// <summary>
	/// Gets or sets the culture used to format and parse strings (en-GB by default).
	/// </summary>
	public static CultureInfo DefaultCulture { get; set; } = CultureInfo.GetCultureInfo("en-GB");
}
