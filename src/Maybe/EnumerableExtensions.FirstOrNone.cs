// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;
using System.Linq;

namespace Monadic;

public static partial class EnumerableExtensions
{
	public static Maybe<T> FirstOrNone<T>(this IEnumerable<T> @this) =>
		@this.FirstOrDefault() switch
		{
			T value =>
				value,

			_ =>
				M.None
		};
}
