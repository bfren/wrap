// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap;

/// <inheritdoc cref="IEither{TEither, TLeft, TRight}"/>
public interface IEither<TLeft, TRight>
{
	/// <summary>
	/// Return the right (correct / valid) value, or the value returned by <paramref name="getValue"/> if this object
	/// holds a left (error / invalid) value.
	/// </summary>
	/// <param name="getValue">Returns the value to use, given the left value.</param>
	/// <returns>The right value, or the value returned by <paramref name="getValue"/>.</returns>
	TRight Unwrap(Func<TLeft, TRight> getValue);

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

/// <summary>
/// Either Monad.
/// </summary>
/// <typeparam name="TEither">Either implementation type.</typeparam>
/// <typeparam name="TLeft">Left (error / invalid) value type.</typeparam>
/// <typeparam name="TRight">Right (correct / valid) value type.</typeparam>
public interface IEither<TEither, TLeft, TRight> : IEither<TLeft, TRight>
	where TEither : IEither<TLeft, TRight>
{
	/// <summary>
	/// Shorthand for returning the current object as a task.
	/// </summary>
	/// <returns>Task result.</returns>
	Task<TEither> AsTask();
}
