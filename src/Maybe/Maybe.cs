// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wrap;

/// <summary>
/// Maybe monad.
/// </summary>
/// <typeparam name="T">Some value type.</typeparam>
public abstract partial record class Maybe<T> : IEither<Maybe<T>, None, T>
{
	/// <inheritdoc/>
	public Task<Maybe<T>> AsTask() =>
		Task.FromResult(this);

	/// <inheritdoc cref="IEither{TLeft, TRight}.GetEnumerator"/>
	public IEnumerator<T> GetEnumerator()
	{
		if (this is Some<T> some)
		{
			yield return some.Value;
		}
	}

	/// <summary>
	/// Convert the current object to a string.
	/// </summary>
	/// <returns>Value string if this is a <see cref="Some{T}"/> or the value type.</returns>
	public sealed override string ToString() =>
		M.Match(this,
			none: $"None: {typeof(T)}",
			some: x =>
				x.ToString() switch
				{
					string value =>
						value,

					_ =>
						$"Some: {typeof(T)}"
				}
		);
}
