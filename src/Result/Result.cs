// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wrap;

/// <summary>
/// Result monad.
/// </summary>
/// <typeparam name="T">Ok value type.</typeparam>
public abstract partial record class Result<T> : IEither<Result<T>, ErrValue, T>
{
	/// <inheritdoc/>
	public Task<Result<T>> AsTask() =>
		Task.FromResult(this);

	/// <inheritdoc cref="IEither{TLeft, TRight}.GetEnumerator"/>
	public IEnumerator<T> GetEnumerator()
	{
		if (this is Ok<T> ok)
		{
			yield return ok.Value;
		}
	}

	/// <summary>
	/// Convert the current object to a string.
	/// </summary>
	/// <returns>Value string if this is a <see cref="Ok{T}"/> or the value type.</returns>
	public sealed override string ToString() =>
		R.Match(this,
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
