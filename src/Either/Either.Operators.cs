// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

public readonly partial struct Either<TLeft, TRight>
{
	/// <summary>
	/// Wrap <paramref name="value"/> as a <see cref="Left{T}"/> - see <see cref="E.WrapLeft{TLeft, TRight}(TLeft)"/>.
	/// </summary>
	/// <remarks>
	/// <see cref="Left{T}"/> and <see cref="Right{T}"/> convert to <see cref="Either{TLeft, TRight}"/> through the union
	/// constructors, so they need no operator here. When <typeparamref name="TLeft"/> and <typeparamref name="TRight"/> are
	/// the same type this conversion is ambiguous - use <see cref="Left{T}"/> or <see cref="Right{T}"/> instead.
	/// </remarks>
	/// <param name="value">Value to wrap.</param>
	/// <returns>A <see cref="Left{T}"/> holding <paramref name="value"/>.</returns>
	public static implicit operator Either<TLeft, TRight>(TLeft value) =>
		new Left<TLeft>(value);

	/// <summary>
	/// Wrap <paramref name="value"/> as a <see cref="Right{T}"/> - see <see cref="E.WrapRight{TLeft, TRight}(TRight)"/>.
	/// </summary>
	/// <remarks>
	/// When <typeparamref name="TLeft"/> and <typeparamref name="TRight"/> are the same type this conversion is ambiguous -
	/// use <see cref="Left{T}"/> or <see cref="Right{T}"/> instead.
	/// </remarks>
	/// <param name="value">Value to wrap.</param>
	/// <returns>A <see cref="Right{T}"/> holding <paramref name="value"/>.</returns>
	public static implicit operator Either<TLeft, TRight>(TRight value) =>
		new Right<TRight>(value);
}
