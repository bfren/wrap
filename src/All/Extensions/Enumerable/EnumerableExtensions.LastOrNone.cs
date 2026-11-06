// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;

namespace Wrap.Extensions;

public static partial class EnumerableExtensions
{
	/// <summary>
	/// Safely get the last value from a list.
	/// </summary>
	/// <typeparam name="T">IEnumerable value type.</typeparam>
	/// <param name="this">IEnumerable object</param>
	/// <returns>Value of the last element of <paramref name="this"/>, or <see cref="None"/>.</returns>
	public static Maybe<T> LastOrNone<T>(this IEnumerable<T> @this)
	{
		// keep only the last item rather than copying the whole sequence
		Maybe<T> last = M.None;
		foreach (var item in @this)
		{
			last = M.Wrap(item);
		}

		return last;
	}
}
