// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions;

public static partial class UnionExtensions
{
	/// <summary>
	/// Convert a <see cref="IUnion{T}"/> to <see cref="Result{T}"/>.
	/// </summary>
	/// <typeparam name="T">Value type.</typeparam>
	/// <param name="this">Union object.</param>
	/// <returns>Result object.</returns>
	public static Result<T> ToResult<T>(this IUnion<T> @this) =>
		R.Wrap(@this.Value);
}
