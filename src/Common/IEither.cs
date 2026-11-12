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
	/// Run <paramref name="fLeft"/> or <paramref name="fRight"/>, depending on which case this object holds, and return its value.
	/// </summary>
	/// <remarks>
	/// Lets generic code (for example <see cref="Unsafe{TEither, TLeft, TRight}"/>) work with any either type:
	/// a union struct holds its case in private fields, so generic code cannot find it with a type test.
	/// Called through a type parameter constrained to this interface, it does not box a struct.
	/// </remarks>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="fLeft">Function to run with the left (error / invalid) value.</param>
	/// <param name="fRight">Function to run with the right (correct / valid) value.</param>
	/// <returns>Value returned by <paramref name="fLeft"/> or <paramref name="fRight"/>.</returns>
	TReturn Match<TReturn>(Func<TLeft, TReturn> fLeft, Func<TRight, TReturn> fRight);
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
