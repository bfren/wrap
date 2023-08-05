// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monadic;

public abstract partial record class Maybe<T> : IEither<None, T>
{
	public Task<Maybe<T>> AsTask() =>
		Task.FromResult(this);

	public IEnumerator<T> GetEnumerator()
	{
		if (this is Some<T> some)
		{
			yield return some.Value;
		}
	}
}
