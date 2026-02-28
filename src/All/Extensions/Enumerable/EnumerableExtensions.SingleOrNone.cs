// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;
using System.Linq;

namespace Wrap.Extensions;

/// <summary>
/// Extension methods for functional monad operations.
/// </summary>
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
		var a = @this.ToArray();
		return (a.Length == 1) switch
		{
			true =>
				a[0],

			false =>
				M.None
		};
	}
}
