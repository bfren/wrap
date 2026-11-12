// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

public static partial class R
{
	/// <summary>
	/// Gets an <see cref="Ok{T}"/> result holding <see langword="true"/>.
	/// </summary>
	public static Result<bool> True =>
		Wrap(true);
}
