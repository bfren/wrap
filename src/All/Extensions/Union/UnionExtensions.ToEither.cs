// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions;

public static partial class UnionExtensions
{
	/// <summary>
	/// Convert a <see cref="IUnion{T}"/> to <see cref="Either{TLeft, TRight}"/>.
	/// </summary>
	/// <typeparam name="TLeft">Left (error / invalid) value type.</typeparam>
	/// <typeparam name="TRight">Right (correct / valid) value type.</typeparam>
	/// <param name="this">Union object.</param>
	/// <returns>Either object.</returns>
	public static Either<TLeft, TRight> ToEither<TLeft, TRight>(this IUnion<TLeft> @this) =>
		E.WrapLeft<TLeft, TRight>(@this.Value);

	/// <summary>
	/// Convert a <see cref="IUnion{T}"/> to <see cref="Either{TLeft, TRight}"/>.
	/// </summary>
	/// <typeparam name="TLeft">Left (error / invalid) value type.</typeparam>
	/// <typeparam name="TRight">Right (correct / valid) value type.</typeparam>
	/// <param name="this">Union object.</param>
	/// <returns>Either object.</returns>
	public static Either<TLeft, TRight> ToEither<TLeft, TRight>(this IUnion<TRight> @this) =>
		E.WrapRight<TLeft, TRight>(@this.Value);
}
