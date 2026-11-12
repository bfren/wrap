// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

public readonly partial struct Either<TLeft, TRight>
{
	#region Match

	/// <inheritdoc/>
	TReturn IEither<TLeft, TRight>.Match<TReturn>(Func<TLeft, TReturn> fLeft, Func<TRight, TReturn> fRight) =>
		this switch
		{
			Left<TLeft> l =>
				fLeft(l.Value),

			Right<TRight> r =>
				fRight(r.Value)
		};

	#endregion

	#region TryGetValue

	/// <summary>
	/// Get the <see cref="Left{T}"/> case, if this object holds one.
	/// </summary>
	/// <remarks>
	/// Called by the compiler for <c>either is Left&lt;TLeft&gt;</c> patterns and <c>switch</c>, so matching does not allocate.
	/// </remarks>
	/// <param name="left">The <see cref="Left{T}"/> case, or a <see cref="Left{T}"/> holding <c>default</c> if this is a <see cref="Right{T}"/>.</param>
	/// <returns><see langword="true"/> if this is a <see cref="Left{T}"/>.</returns>
	public bool TryGetValue(out Left<TLeft> left)
	{
		left = new(this.left!);
		return !isRight;
	}

	/// <summary>
	/// Get the <see cref="Right{T}"/> case, if this object holds one.
	/// </summary>
	/// <remarks>
	/// Called by the compiler for <c>either is Right&lt;TRight&gt;</c> patterns and <c>switch</c>, so matching does not allocate.
	/// </remarks>
	/// <param name="right">The <see cref="Right{T}"/> case, or a <see cref="Right{T}"/> holding <c>default</c> if this is a <see cref="Left{T}"/>.</param>
	/// <returns><see langword="true"/> if this is a <see cref="Right{T}"/>.</returns>
	public bool TryGetValue(out Right<TRight> right)
	{
		right = new(this.right!);
		return isRight;
	}

	#endregion

	#region Unwrap

	/// <summary>
	/// Return the value if this is a <see cref="Right{T}"/>, otherwise the value returned by <paramref name="getValue"/>.
	/// </summary>
	/// <param name="getValue">Returns the value to use, given the <see cref="Left{T}"/> value.</param>
	/// <returns>The <see cref="Right{T}"/> value, or the value returned by <paramref name="getValue"/>.</returns>
	public TRight Unwrap(Func<TLeft, TRight> getValue) =>
		this switch
		{
			Left<TLeft> l =>
				getValue(l.Value),

			Right<TRight> r =>
				r.Value
		};

	#endregion

	#region ToString

	/// <summary>
	/// Convert the current object to a string.
	/// </summary>
	/// <returns>The string of the current case, for example <c>Left { Value = bad }</c> or <c>Right { Value = 42 }</c>.</returns>
	public override string ToString() =>
		this switch
		{
			Left<TLeft> l =>
				l.ToString(),

			Right<TRight> r =>
				r.ToString()
		};

	#endregion
}
