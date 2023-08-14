// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Monadic;

public static partial class M
{
	/// <summary>
	/// Create a <see cref="None"/> value.
	/// </summary>
	public static None None =>
		new();
}
