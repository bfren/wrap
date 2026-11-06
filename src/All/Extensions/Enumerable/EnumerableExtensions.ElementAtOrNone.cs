// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;

namespace Wrap.Extensions;

public static partial class EnumerableExtensions
{
	/// <summary>
	/// Safely get a value from a list.
	/// </summary>
	/// <typeparam name="T">IEnumerable value type.</typeparam>
	/// <param name="this">IEnumerable object.</param>
	/// <param name="index">Index of value to return.</param>
	/// <returns>Value of the element at position <paramref name="index"/> of <paramref name="this"/>, or <see cref="None"/>.</returns>
	public static Maybe<T> ElementAtOrNone<T>(this IEnumerable<T> @this, int index)
	{
		if (index < 0)
		{
			return M.None;
		}

		// stop at the requested item - ToArray() would read the whole sequence
		foreach (var item in @this)
		{
			if (index-- == 0)
			{
				return M.Wrap(item);
			}
		}

		return M.None;
	}
}
