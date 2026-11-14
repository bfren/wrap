// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;
using System.Linq;

namespace Wrap.Extensions;

public static partial class EnumerableExtensions
{
	/// <summary>
	/// Get the first item, without throwing if the list is empty.
	/// </summary>
	/// <typeparam name="T">List item type.</typeparam>
	/// <param name="this">List of values.</param>
	/// <returns><see cref="Some{T}"/> holding the first item, or <see cref="None"/> if the list is empty or the first item is <see langword="null"/>.</returns>
	public static Maybe<T> FirstOrNone<T>(this IEnumerable<T> @this) =>
		@this.Select(M.Wrap).FirstOrDefault();
}
