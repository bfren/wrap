// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;
using System.Linq;

namespace Wrap.Extensions;

public static partial class EnumerableExtensions
{
	/// <summary>
	/// Get the item at <paramref name="index"/>, without throwing if there isn't one.
	/// </summary>
	/// <typeparam name="T">List item type.</typeparam>
	/// <param name="this">List of values.</param>
	/// <param name="index">Zero-based index of the item to return.</param>
	/// <returns><see cref="Some{T}"/> holding the item, or <see cref="None"/> if <paramref name="index"/> is out of range or the item is <see langword="null"/>.</returns>
	public static Maybe<T> ElementAtOrNone<T>(this IEnumerable<T> @this, int index) =>
		@this.Select(M.Wrap).ElementAtOrDefault(index);
}
