// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions;

public static partial class MonadExtensions
{
	/// <summary>
	/// Convert an <see cref="IMonad{T}"/> to an <see cref="Either{TLeft, TRight}"/> holding its value as the Left value.
	/// </summary>
	/// <typeparam name="TLeft">Left (error / invalid) value type.</typeparam>
	/// <typeparam name="TRight">Right (correct / valid) value type.</typeparam>
	/// <param name="this">Monad object.</param>
	/// <returns>A <see cref="Left{T}"/> holding the value of <paramref name="this"/>.</returns>
	public static Either<TLeft, TRight> ToEither<TLeft, TRight>(this IMonad<TLeft> @this) =>
		E.WrapLeft<TLeft, TRight>(@this.Value);

	/// <summary>
	/// Convert an <see cref="IMonad{T}"/> to an <see cref="Either{TLeft, TRight}"/> holding its value as the Right value.
	/// </summary>
	/// <typeparam name="TLeft">Left (error / invalid) value type.</typeparam>
	/// <typeparam name="TRight">Right (correct / valid) value type.</typeparam>
	/// <param name="this">Monad object.</param>
	/// <returns>A <see cref="Right{T}"/> holding the value of <paramref name="this"/>.</returns>
	public static Either<TLeft, TRight> ToEither<TLeft, TRight>(this IMonad<TRight> @this) =>
		E.WrapRight<TLeft, TRight>(@this.Value);
}
