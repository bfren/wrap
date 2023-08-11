// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;
using System.Linq;

namespace Monadic;

public static partial class EnumerableExtensions
{
	public static Maybe<T> ElementAtOrNone<T>(this IEnumerable<T> @this, int index) =>
		@this.ElementAtOrDefault(index) switch
		{
			T value =>
				value,

			_ =>
				M.None
		};
}
