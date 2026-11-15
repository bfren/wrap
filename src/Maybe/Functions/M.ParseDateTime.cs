// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Globalization;

namespace Wrap;

public static partial class M
{
	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<DateOnly> ParseDateOnly(string? input) =>
		Parse(input, (string? s, out DateOnly result) =>
			DateOnly.TryParse(s, F.DefaultCulture, DateTimeStyles.None, out result)
		);

	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<DateOnly> ParseDateOnly(ReadOnlySpan<char> input) =>
		Parse(input, (ReadOnlySpan<char> s, out DateOnly result) =>
			DateOnly.TryParse(s, F.DefaultCulture, DateTimeStyles.None, out result)
		);

	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<DateTime> ParseDateTime(string? input) =>
		Parse(input, (string? s, out DateTime result) =>
			DateTime.TryParse(s, F.DefaultCulture, DateTimeStyles.RoundtripKind, out result)
		);

	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<DateTime> ParseDateTime(ReadOnlySpan<char> input) =>
		Parse(input, (ReadOnlySpan<char> s, out DateTime result) =>
			DateTime.TryParse(s, F.DefaultCulture, DateTimeStyles.RoundtripKind, out result)
		);

	/// <inheritdoc cref="ParseBool(string?)"/>
	/// <remarks>
	/// Input without an offset is treated as UTC, not server local time, so the result is the same on every machine.
	/// </remarks>
	public static Maybe<DateTimeOffset> ParseDateTimeOffset(string? input) =>
		Parse(input, (string? s, out DateTimeOffset result) =>
			DateTimeOffset.TryParse(s, F.DefaultCulture, DateTimeStyles.AssumeUniversal, out result)
		);

	/// <inheritdoc cref="ParseDateTimeOffset(string?)"/>
	public static Maybe<DateTimeOffset> ParseDateTimeOffset(ReadOnlySpan<char> input) =>
		Parse(input, (ReadOnlySpan<char> s, out DateTimeOffset result) =>
			DateTimeOffset.TryParse(s, F.DefaultCulture, DateTimeStyles.AssumeUniversal, out result)
		);

	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<TimeOnly> ParseTimeOnly(string? input) =>
		Parse(input, (string? s, out TimeOnly result) =>
			TimeOnly.TryParse(s, F.DefaultCulture, DateTimeStyles.None, out result)
		);

	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<TimeOnly> ParseTimeOnly(ReadOnlySpan<char> input) =>
		Parse(input, (ReadOnlySpan<char> s, out TimeOnly result) =>
			TimeOnly.TryParse(s, F.DefaultCulture, DateTimeStyles.None, out result)
		);
}
