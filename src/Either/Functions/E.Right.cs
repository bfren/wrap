// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Monadic;

public static partial class E
{
	/// <summary>
	/// Create a right (correct / valid) value.
	/// </summary>
	/// <typeparam name="TLeft">Left (error / invalid) value type.</typeparam>
	/// <typeparam name="TRight">Right (correct / valid) value type.</typeparam>
	/// <param name="value">Left (error / invalid) value.</param>
	/// <returns><see cref="Monadic.Right{TLeft, TRight}"/> with value <paramref name="value"/>.</returns>
	public static Either<TLeft, TRight> Right<TLeft, TRight>(TRight value) =>
		new Right<TLeft, TRight>(value);
}
