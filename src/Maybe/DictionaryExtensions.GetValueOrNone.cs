// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;

namespace Monadic;

public static partial class DictionaryExtensions
{
	public static Maybe<TValue> GetValueOrNone<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key) =>
		@this.TryGetValue(key, out var value) switch
		{
			false =>
				M.None,

			true =>
				value
		};
}
