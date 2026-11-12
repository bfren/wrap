// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

public static partial class R
{
	/// <summary>
	/// Gets an <see cref="Ok{T}"/> result holding <see langword="false"/> - it is not a <see cref="Failure"/>.
	/// </summary>
	public static Result<bool> False =>
		Wrap(false);
}
