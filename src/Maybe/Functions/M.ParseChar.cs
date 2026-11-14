// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

public static partial class M
{
	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<char> ParseChar(string? input) =>
		input is [var c] ? c : None;

	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<char> ParseChar(ReadOnlySpan<char> input) =>
		input is [var c] ? c : None;
}
