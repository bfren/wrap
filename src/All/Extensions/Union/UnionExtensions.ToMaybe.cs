// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions;

public static partial class UnionExtensions
{
	/// <summary>
	/// Convert a <see cref="IUnion{T}"/> to <see cref="Maybe{T}"/>.
	/// </summary>
	/// <typeparam name="T">Value type.</typeparam>
	/// <param name="this">Union object.</param>
	/// <returns>Maybe object.</returns>
	public static Maybe<T> ToMaybe<T>(this IUnion<T> @this) =>
		M.Wrap(@this.Value);
}
