// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wrap;

/// <inheritdoc cref="IEither{TLeft, TRight}"/>
public abstract record class Either<TLeft, TRight> : IEither<Either<TLeft, TRight>, TLeft, TRight>
{
	/// <inheritdoc/>
	public Task<Either<TLeft, TRight>> AsTask() =>
		Task.FromResult(this);

	/// <inheritdoc/>
	public TRight Unwrap(Func<TLeft, TRight> getValue) =>
		E.Match<Either<TLeft, TRight>, TLeft, TRight, TRight>(this,
			left: l => getValue(l),
			right: x => x
		);

	/// <inheritdoc cref="IEither{TLeft, TRight}.GetEnumerator"/>
	public IEnumerator<TRight> GetEnumerator()
	{
		if (this is Right<TLeft, TRight> right)
		{
			yield return right.Value;
		}
	}
}
