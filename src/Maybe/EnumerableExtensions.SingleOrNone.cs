// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;
using System.Linq;

namespace Wrap;

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
	public static Maybe<T> SingleOrNone<T>(this IEnumerable<T> @this) =>
		@this.Count() switch
		{
			1 =>
				@this.Single(),

			_ =>
				M.None
		};
}
