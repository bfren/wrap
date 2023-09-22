// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

public static partial class M
{
	/// <summary>
	/// Create a new <see cref="Wrap.None"/> value.
	/// </summary>
	public static None None =>
		new();
}
