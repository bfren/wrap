// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

public static partial class M
{
	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<Guid> ParseGuid(string? input) =>
		Parse<Guid>(input, Guid.TryParse);

	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<Guid> ParseGuid(ReadOnlySpan<char> input) =>
		Parse<Guid>(input, Guid.TryParse);
}
