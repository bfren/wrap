// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Testing;

public static partial class EitherExtensions
{
	/// <summary>
	/// Assert that <paramref name="this"/> is a <see cref="Left{T}"/> and return its value.
	/// </summary>
	/// <typeparam name="TLeft">Left (error / invalid) value type.</typeparam>
	/// <typeparam name="TRight">Right (correct / valid) value type.</typeparam>
	/// <param name="this">Either object.</param>
	/// <returns>The <see cref="Left{T}"/> value.</returns>
	public static TLeft AssertLeft<TLeft, TRight>(this Either<TLeft, TRight> @this) =>
		Assert.IsType<Left<TLeft>>(@this.Value).Value; // the union's boxed case: @this itself boxes as Either<TLeft, TRight>

	/// <summary>
	/// Assert that <paramref name="this"/> is a <see cref="Left{T}"/> and its value matches <paramref name="expected"/>.
	/// </summary>
	/// <typeparam name="TLeft">Left (error / invalid) value type.</typeparam>
	/// <typeparam name="TRight">Right (correct / valid) value type.</typeparam>
	/// <param name="this">Either object.</param>
	/// <param name="expected">Expected value.</param>
	public static void AssertLeft<TLeft, TRight>(this Either<TLeft, TRight> @this, TLeft expected) =>
		Assert.Equal(expected, AssertLeft(@this));
}
