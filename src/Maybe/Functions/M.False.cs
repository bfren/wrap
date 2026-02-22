// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

public static partial class M
{
	/// <summary>
	/// Gets a value that represents <see langword="false"/>.
	/// </summary>
	public static Maybe<bool> False =>
		Wrap(false);
}
