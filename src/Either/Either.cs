// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monadic;

public abstract partial record class Either<TLeft, TRight> : IEither<TLeft, TRight>
{
	public Task<Either<TLeft, TRight>> AsTask() =>
		Task.FromResult(this);

	public IEnumerator<TRight> GetEnumerator()
	{
		if (this is Right<TLeft, TRight> right)
		{
			yield return right.Value;
		}
	}
}
