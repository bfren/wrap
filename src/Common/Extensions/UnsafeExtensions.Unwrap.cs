// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions;

public static partial class UnsafeExtensions
{
	public static TRight UnwrapEither<TEither, TLeft, TRight>(this Unsafe2<TEither, TLeft, TRight> @this)
		where TEither : IEither<TEither, TLeft, TRight> =>
		@this.Value switch
		{
			IRight<TLeft, TRight> left =>
				left.Value,

			_ =>
				default!
		};
}
