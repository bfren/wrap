// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Globalization;

namespace Monadic;

/// <summary>
/// Pure functions for interacting with <see cref="Maybe{T}"/> types.
/// </summary>
public static partial class M
{
	/// <summary>
	/// Default culture (en-GB) - used when parsing strings
	/// </summary>
	public static CultureInfo DefaultCulture { get; set; } = CultureInfo.GetCultureInfo("en-GB");
}
