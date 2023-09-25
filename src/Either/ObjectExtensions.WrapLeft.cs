// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

public static partial class ObjectExtensions
{
	/// <inheritdoc cref="E.WrapLeft{TLeft, TRight}(TLeft)"/>
	public static Either<TLeft, TRight> WrapLeft<TLeft, TRight>(this TLeft value) =>
		E.WrapLeft<TLeft, TRight>(value);
}
