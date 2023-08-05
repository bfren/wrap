// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Monadic;

public static partial class M
{
	public static Maybe<T> Wrap<T>(T value) =>
		value switch
		{
			T x =>
				new Some<T>(x),

			_ =>
				None
		};
}
