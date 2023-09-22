// Monads: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monads;

/// <inheritdoc cref="IEither{TLeft, TRight}"/>
public abstract partial record class Either<TLeft, TRight> : IEither<TLeft, TRight>
{
	/// <summary>
	/// Shorthand for returning the current object as a task.
	/// </summary>
	/// <returns>Task result.</returns>
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
