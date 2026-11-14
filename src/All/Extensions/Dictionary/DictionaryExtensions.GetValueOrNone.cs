// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;

namespace Wrap.Extensions;

public static partial class DictionaryExtensions
{
	/// <summary>
	/// Get the value stored under <paramref name="key"/>, without throwing if there isn't one.
	/// </summary>
	/// <typeparam name="TKey">Key type.</typeparam>
	/// <typeparam name="TValue">Value type.</typeparam>
	/// <param name="this">Dictionary object.</param>
	/// <param name="key">Key whose value to get.</param>
	/// <returns><see cref="Some{T}"/> holding the value, or <see cref="None"/> if there is no entry for <paramref name="key"/> or its value is <see langword="null"/>.</returns>
	public static Maybe<TValue> GetValueOrNone<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key) =>
		@this.TryGetValue(key, out var value) switch
		{
			false =>
				M.None,

			true =>
				value
		};
}
