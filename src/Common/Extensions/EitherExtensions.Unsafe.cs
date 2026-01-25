// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions;

public static partial class EitherExtensions
{

	public static Unsafe2<TEither, TLeft, TRight> Unsafe2<TEither, TLeft, TRight>(this TEither @this)
		where TEither : IEither<TEither, TLeft, TRight> =>
		new(@this);
}
