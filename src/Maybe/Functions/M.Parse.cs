// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

public static partial class M
{
	/// <summary>
	/// A <c>TryParse</c> function that reads a span of characters.
	/// </summary>
	/// <typeparam name="T">Parsed value type.</typeparam>
	/// <param name="input">Input value.</param>
	/// <param name="result">Parsed value, if parsing succeeds.</param>
	/// <returns><see langword="true"/> if parsing succeeds.</returns>
	internal delegate bool TryParseSpan<T>(ReadOnlySpan<char> input, out T result);

	/// <summary>
	/// A <c>TryParse</c> function that reads a string.
	/// </summary>
	/// <typeparam name="T">Parsed value type.</typeparam>
	/// <param name="input">Input value.</param>
	/// <param name="result">Parsed value, if parsing succeeds.</param>
	/// <returns><see langword="true"/> if parsing succeeds.</returns>
	internal delegate bool TryParseString<T>(string? input, out T result);

	/// <summary>
	/// Attempt to parse <paramref name="value"/> using <paramref name="tryParse"/>.
	/// </summary>
	/// <typeparam name="T">Parsed value type.</typeparam>
	/// <param name="value">Input value.</param>
	/// <param name="tryParse">Parse function.</param>
	/// <returns>The parsed value on success, otherwise <see cref="Wrap.None"/>.</returns>
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
