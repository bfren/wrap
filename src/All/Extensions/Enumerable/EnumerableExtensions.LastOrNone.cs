// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;
using System.Linq;

namespace Wrap.Extensions;

public static partial class EnumerableExtensions
{
	/// <summary>
	/// Safely get the last value from a list.
	/// </summary>
	/// <typeparam name="T">IEnumerable value type.</typeparam>
	/// <param name="this">IEnumerable object</param>
	/// <returns>Value of the last element of <paramref name="this"/>, or <see cref="None"/>.</returns>
	public static Maybe<T> LastOrNone<T>(this IEnumerable<T> @this) =>
		@this.LastOrDefault() switch
		{
			T value =>
				value,

			_ =>
				M.None
		};
}
