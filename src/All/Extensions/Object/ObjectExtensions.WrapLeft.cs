// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions;

public static partial class ObjectExtensions
{
	/// <inheritdoc cref="E.WrapLeft{TLeft, TRight}(TLeft)"/>
	public static Either<TLeft, TRight> WrapLeft<TLeft, TRight>(this TLeft value) =>
		E.WrapLeft<TLeft, TRight>(value);
}
