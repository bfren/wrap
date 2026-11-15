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
	/// <remarks>
	/// This is one setting for the whole process, not per request or per thread. Set it once at startup to the culture
	/// your app expects: for example, the MVC model binders use it to parse dates and numbers for every client, so
	/// with the default <c>03/04/2026</c> is read as 3 April.
	/// </remarks>
	public static CultureInfo DefaultCulture { get; set; } =
		CultureInfo.GetCultureInfo("en-GB");
}
