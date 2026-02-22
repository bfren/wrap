// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

public static partial class R
{
	/// <summary>
	/// Gets a result that represents a failed operation with a value of <see langword="false"/>.
	/// </summary>
	public static Result<bool> False =>
		Wrap(false);
}
