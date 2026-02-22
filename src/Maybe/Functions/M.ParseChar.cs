// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

public static partial class M
{
	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<char> ParseChar(string? input) =>
		Parse<char>(input, char.TryParse);

	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<char> ParseChar(ReadOnlySpan<char> input) =>
		Parse<char>(input, (x, out y) => char.TryParse(x.ToString(), out y));
}
