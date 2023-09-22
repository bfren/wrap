// Monads: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Monads;

public static partial class M
{
	/// <summary>
	/// Create a new <see cref="Monads.None"/> value.
	/// </summary>
	public static None None =>
		new();
}
