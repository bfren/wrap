// Monads: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Monads;

public static partial class M
{
	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<char> ParseChar(string? input) =>
		Parse<char>(input, char.TryParse);
}
