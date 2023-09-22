// Monads: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Monads;

public static partial class M
{
	/// <summary>
	/// Attempt to parse the specified <paramref name="input"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// If the parse succeeds, the value is returned - otherwise <see cref="Monads.None"/>.
	/// </para>
	/// </remarks>
	/// <param name="input">Input value.</param>
	/// <returns>The parsed value on success - otherwise <see cref="Monads.None"/>.</returns>
	public static Maybe<bool> ParseBool(string? input) =>
		Parse<bool>(input, bool.TryParse);

	/// <inheritdoc cref="ParseBool(string?)"/>
	public static Maybe<bool> ParseBool(ReadOnlySpan<char> input) =>
		Parse<bool>(input, bool.TryParse);
}
