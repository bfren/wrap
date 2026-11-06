// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;

namespace Wrap.Extensions;

public static partial class EnumerableExtensions
{
	/// <summary>
	/// Safely get the first value from a list.
	/// </summary>
	/// <typeparam name="T">IEnumerable value type.</typeparam>
	/// <param name="this">IEnumerable object</param>
	/// <returns>Value of the first element of <paramref name="this"/>, or <see cref="None"/>.</returns>
	public static Maybe<T> FirstOrNone<T>(this IEnumerable<T> @this)
	{
		// stop at the first item - ToArray() would read the whole sequence (e.g. a full database table)
		foreach (var item in @this)
		{
			return M.Wrap(item);
		}

		return M.None;
	}
}
