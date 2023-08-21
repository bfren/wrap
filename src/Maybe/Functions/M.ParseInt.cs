// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Monadic;

public static partial class M
{
	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<short> ParseInt16(string? input) =>
		Parse(input, (string? s, out short r) => short.TryParse(s, IntegerNumberStyles, DefaultCulture, out r));

	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<short> ParseInt16(ReadOnlySpan<char> input) =>
		Parse(input, (ReadOnlySpan<char> s, out short r) => short.TryParse(s, IntegerNumberStyles, DefaultCulture, out r));

	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<int> ParseInt32(string? input) =>
		Parse(input, (string? s, out int r) => int.TryParse(s, IntegerNumberStyles, DefaultCulture, out r));

	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<int> ParseInt32(ReadOnlySpan<char> input) =>
		Parse(input, (ReadOnlySpan<char> s, out int r) => int.TryParse(s, IntegerNumberStyles, DefaultCulture, out r));

	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<long> ParseInt64(string? input) =>
		Parse(input, (string? s, out long r) => long.TryParse(s, IntegerNumberStyles, DefaultCulture, out r));

	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<long> ParseInt64(ReadOnlySpan<char> input) =>
		Parse(input, (ReadOnlySpan<char> s, out long r) => long.TryParse(s, IntegerNumberStyles, DefaultCulture, out r));

	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<nint> ParseIntPtr(string? input) =>
		Parse(input, (string? s, out nint r) => nint.TryParse(s, IntegerNumberStyles, DefaultCulture, out r));

	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<nint> ParseIntPtr(ReadOnlySpan<char> input) =>
		Parse(input, (ReadOnlySpan<char> s, out nint r) => nint.TryParse(s, IntegerNumberStyles, DefaultCulture, out r));

	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<ushort> ParseUInt16(string? input) =>
		Parse(input, (string? s, out ushort r) => ushort.TryParse(s, IntegerNumberStyles, DefaultCulture, out r));

	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<ushort> ParseUInt16(ReadOnlySpan<char> input) =>
		Parse(input, (ReadOnlySpan<char> s, out ushort r) => ushort.TryParse(s, IntegerNumberStyles, DefaultCulture, out r));

	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<uint> ParseUInt32(string? input) =>
		Parse(input, (string? s, out uint r) => uint.TryParse(s, IntegerNumberStyles, DefaultCulture, out r));

	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<uint> ParseUInt32(ReadOnlySpan<char> input) =>
		Parse(input, (ReadOnlySpan<char> s, out uint r) => uint.TryParse(s, IntegerNumberStyles, DefaultCulture, out r));

	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<ulong> ParseUInt64(string? input) =>
		Parse(input, (string? s, out ulong r) => ulong.TryParse(s, IntegerNumberStyles, DefaultCulture, out r));

	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<ulong> ParseUInt64(ReadOnlySpan<char> input) =>
		Parse(input, (ReadOnlySpan<char> s, out ulong r) => ulong.TryParse(s, IntegerNumberStyles, DefaultCulture, out r));

	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<nuint> ParseUIntPtr(string? input) =>
		Parse(input, (string? s, out nuint r) => nuint.TryParse(s, IntegerNumberStyles, DefaultCulture, out r));

	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<nuint> ParseUIntPtr(ReadOnlySpan<char> input) =>
		Parse(input, (ReadOnlySpan<char> s, out nuint r) => nuint.TryParse(s, IntegerNumberStyles, DefaultCulture, out r));
}
