// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

public static partial class E
{
	/// <summary>
	/// Create a right (correct / valid) value.
	/// </summary>
	/// <typeparam name="TLeft">Left (error / invalid) value type.</typeparam>
	/// <typeparam name="TRight">Right (correct / valid) value type.</typeparam>
	/// <param name="value">Left (error / invalid) value.</param>
	/// <returns><see cref="Right{TLeft, TRight}"/> with value <paramref name="value"/>.</returns>
	public static Either<TLeft, TRight> WrapRight<TLeft, TRight>(TRight value) =>
		new Right<TLeft, TRight>(value);
}
