// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Globalization;

namespace Wrap;

/// <summary>
/// Pure functions for interacting with <see cref="Maybe{T}"/> types.
/// </summary>
public static partial class M
{
	/// <summary>
	/// Default number style for formatting floating-point numbers - see <see cref="ParseDouble(string?)"/> etc.
	/// </summary>
	internal static NumberStyles FloatNumberStyles { get; } = NumberStyles.Float | NumberStyles.AllowThousands;

	/// <summary>
	/// Default number style for parsing integers - see <see cref="ParseInt64(string?)"/> etc.
	/// </summary>
	internal static NumberStyles IntegerNumberStyles { get; } = NumberStyles.Integer;
}
