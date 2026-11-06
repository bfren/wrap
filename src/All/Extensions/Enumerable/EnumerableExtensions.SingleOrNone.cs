// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;

namespace Wrap.Extensions;

public static partial class EnumerableExtensions
{
	/// <summary>
	/// Safely get the single value of a list.
	/// </summary>
	/// <typeparam name="T">IEnumerable value type.</typeparam>
	/// <param name="this">IEnumerable object</param>
	/// <returns>
	/// Value of the single element of <paramref name="this"/>, or <see cref="None"/> if the list is empty
	/// or contains more than one element.
	/// </returns>
	public static Maybe<T> SingleOrNone<T>(this IEnumerable<T> @this)
	{
		// stop after the second item - more than one means None
		using var e = @this.GetEnumerator();
		if (!e.MoveNext())
		{
			return M.None;
		}

		var first = e.Current;
		return e.MoveNext() ? M.None : M.Wrap(first);
	}
}
