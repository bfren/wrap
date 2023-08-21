// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Monadic;

public static partial class M
{
	/// <summary>
	/// Attempt to parse the specified <paramref name="input"/>.
	/// </summary>
	/// <typeparam name="T">Result type.</typeparam>
	/// <param name="input">Input value.</param>
	/// <param name="result">Result value.</param>
	internal delegate bool TryParseSpan<T>(ReadOnlySpan<char> input, out T result);

	/// <inheritdoc cref="ParseBool(string?)"/>
	internal delegate bool TryParseString<T>(string? input, out T result);

	/// <summary>
	/// Attempt to parse the specified <paramref name="value"/> using <paramref name="tryParse"/>.
	/// </summary>
	/// <typeparam name="T">Result type.</typeparam>
	/// <param name="value">Input value.</param>
	/// <param name="tryParse">Parse function.</param>
	internal static Maybe<T> Parse<T>(ReadOnlySpan<char> value, TryParseSpan<T> tryParse) =>
		tryParse(value, out var result) switch
		{
			false =>
				None,

			true =>
				result
		};

	/// <inheritdoc cref="Parse{T}(ReadOnlySpan{char}, TryParseSpan{T})"/>
	internal static Maybe<T> Parse<T>(string? value, TryParseString<T> tryParse) =>
		tryParse(value, out var result) switch
		{
			false =>
				None,

			true =>
				result
		};
}
