// Monads: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monads;

public abstract partial record class Result<T> : IEither<Exception, T>
{
	public Task<Result<T>> AsTask() =>
		Task.FromResult(this);

	public IEnumerator<T> GetEnumerator()
	{
		if (this is Ok<T> ok)
		{
			yield return ok.Value;
		}
	}

	public sealed override string ToString() =>
		R.Switch(this,
			err: x => x.Message,
			ok: x => x?.ToString() switch
			{
				string value =>
					value,

				_ =>
					$"OK: {typeof(T)}"
			}
		);
}
