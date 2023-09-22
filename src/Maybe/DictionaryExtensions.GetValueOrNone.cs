// Monads: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;

namespace Monads;

public static partial class DictionaryExtensions
{
	/// <summary>
	/// Gets the value associated with the specified key.
	/// </summary>
	/// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
	/// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
	/// <param name="this">Dictionary object.</param>
	/// <param name="key">The key whose value to get.</param>
	/// <returns>The value associated with <paramref name="key"/>, or <see cref="None"/>.</returns>
	public static Maybe<TValue> GetValueOrNone<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key) =>
		@this.TryGetValue(key, out var value) switch
		{
			false =>
				M.None,

			true =>
				value
		};
}
