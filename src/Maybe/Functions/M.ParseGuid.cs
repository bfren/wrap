// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Monadic;

public static partial class M
{
	/// <inheritdoc cref="TryParseSpan{T}"/>
	public static Maybe<Guid> ParseGuid(string? input) =>
		Parse<Guid>(input, Guid.TryParse);

	/// <inheritdoc cref="TryParseSpan{T}"/>
	public static Maybe<Guid> ParseGuid(ReadOnlySpan<char> input) =>
		Parse<Guid>(input, Guid.TryParse);
}
