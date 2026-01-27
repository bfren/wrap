// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;
using System.Linq;

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
	public static Maybe<T> ElementAtOrNone<T>(this IEnumerable<T> @this, int index) =>
		(@this.Count() > index) switch
		{
			true =>
				@this.ElementAt(index),

			false =>
				M.None
		};
}
