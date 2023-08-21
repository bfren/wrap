// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Monadic;

public static partial class M
{
	/// <summary>
	/// Create a new <see cref="Monadic.None"/> value.
	/// </summary>
	public static None None =>
		new();
}
