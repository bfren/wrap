// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;
using System.Linq;

namespace Wrap.Extensions;

public static partial class EnumerableExtensions
{
	/// <summary>
	/// Get the only item, without throwing if there are none or more than one.
	/// </summary>
	/// <remarks>
	/// At most two items are read.
	/// </remarks>
	/// <typeparam name="T">List item type.</typeparam>
	/// <param name="this">List of values.</param>
	/// <returns>
	/// <see cref="Some{T}"/> holding the only item, or <see cref="None"/> if the list is empty, has more than one item,
	/// or its only item is <see langword="null"/>.
	/// </returns>
	public static Maybe<T> SingleOrNone<T>(this IEnumerable<T> @this) =>
		// can't use SingleOrDefault() because it throws an exception if there is more than one element
		@this.Select(M.Wrap).Take(2).ToArray() switch
		{
			[var x] =>
				x,

			_ =>
				M.None
		};
}
