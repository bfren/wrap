// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Either Monad: holds a left (error / invalid) or a right (correct / valid) value.
/// </summary>
/// <typeparam name="TLeft">Left (error / invalid) value type.</typeparam>
/// <typeparam name="TRight">Right (correct / valid) value type.</typeparam>
public interface IEither<TLeft, TRight>
{
	/// <summary>
	/// Get the left (error / invalid) value, if this object holds one.
	/// </summary>
	/// <remarks>
	/// Lets generic code (for example <see cref="Unsafe{TEither, TLeft, TRight}"/>) read any either type directly:
	/// a union struct holds its case in private fields, so generic code cannot find it with a type test.
	/// Called through a type parameter constrained to this interface, it does not box a struct or allocate.
	/// </remarks>
	/// <param name="left">The left value, or <c>default</c> if this object holds a right value.</param>
	/// <returns><see langword="true"/> if this object holds a left value.</returns>
	bool TryGetLeft(out TLeft left);

	/// <summary>
	/// Get the right (correct / valid) value, if this object holds one.
	/// </summary>
	/// <remarks>
	/// <inheritdoc cref="TryGetLeft(out TLeft)" path="/remarks"/>
	/// </remarks>
	/// <param name="right">The right value, or <c>default</c> if this object holds a left value.</param>
	/// <returns><see langword="true"/> if this object holds a right value.</returns>
	bool TryGetRight(out TRight right);
}
