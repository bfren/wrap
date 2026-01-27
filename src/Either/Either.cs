// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wrap;

/// <inheritdoc cref="IEither{TLeft, TRight}"/>
public abstract record class Either<TLeft, TRight> : IEither<Either<TLeft, TRight>, TLeft, TRight>
{
	/// <inheritdoc/>
	public Task<Either<TLeft, TRight>> AsTask() =>
		Task.FromResult(this);

	/// <inheritdoc cref="IEither{TLeft, TRight}.GetEnumerator"/>
	public IEnumerator<TRight> GetEnumerator()
	{
		if (this is Right<TLeft, TRight> right)
		{
			yield return right.Value;
		}
	}
}
