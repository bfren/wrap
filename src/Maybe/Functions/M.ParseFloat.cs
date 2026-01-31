// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

public static partial class M
{
	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<decimal> ParseDecimal(string? input) =>
		Parse(input, (string? s, out decimal r) => decimal.TryParse(s, FloatNumberStyles, F.DefaultCulture, out r));

	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<decimal> ParseDecimal(ReadOnlySpan<char> input) =>
		Parse(input, (ReadOnlySpan<char> s, out decimal r) => decimal.TryParse(s, FloatNumberStyles, F.DefaultCulture, out r));

	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<double> ParseDouble(string? input) =>
		Parse(input, (string? s, out double r) => double.TryParse(s, FloatNumberStyles, F.DefaultCulture, out r));

	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<double> ParseDouble(ReadOnlySpan<char> input) =>
		Parse(input, (ReadOnlySpan<char> s, out double r) => double.TryParse(s, FloatNumberStyles, F.DefaultCulture, out r));

	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<float> ParseSingle(string? input) =>
		Parse(input, (string? s, out float r) => float.TryParse(s, FloatNumberStyles, F.DefaultCulture, out r));

	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<float> ParseSingle(ReadOnlySpan<char> input) =>
		Parse(input, (ReadOnlySpan<char> s, out float r) => float.TryParse(s, FloatNumberStyles, F.DefaultCulture, out r));
}
