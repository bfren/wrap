// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions;

public static partial class ObjectExtensions
{
	/// <inheritdoc cref="E.WrapRight{TLeft, TRight}(TRight)"/>
	public static Either<TLeft, TRight> WrapRight<TLeft, TRight>(this TRight value) =>
		E.WrapRight<TLeft, TRight>(value);
}
