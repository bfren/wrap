// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wrap;

/// <summary>
/// Maybe monad.
/// </summary>
/// <typeparam name="T">Some value type.</typeparam>
public abstract partial record class Maybe<T> : IEither<Maybe<T>, None, T>, IEquatable<Maybe<T>>
{
	/// <summary>
	/// Returns true if this object is a <see cref="None"/>.
	/// </summary>
	public bool IsNone =>
		!IsSome;

	/// <summary>
	/// Returns true if this object is a <see cref="Some{T}"/>.
	/// </summary>
	public bool IsSome =>
		this is Some<T>;

	/// <inheritdoc/>
	public Task<Maybe<T>> AsTask() =>
		Task.FromResult(this);

	/// <inheritdoc cref="Unwrap(Func{None, T})"/>
	public T Unwrap(Func<T> getValue) =>
		Unwrap(_ => getValue());

	/// <inheritdoc/>
	public T Unwrap(Func<None, T> getValue) =>
		M.Match(this,
			fNone: () => getValue(M.None),
			fSome: x => x
		);

	/// <summary>
	/// Use enumerator pattern to get <typeparamref name="T"/> value.
	/// </summary>
	/// <remarks>
	/// <para>
	/// For example:
	/// </para>
	/// <code>
	/// foreach (var some in maybe) {
	///     // if 'maybe' is <see cref="None"/>, the loop is empty
	///     // otherwise 'some' is <typeparamref name="T"/><br/>
	/// }
	/// </code>
	/// </remarks>
	/// <returns>Enumerator containing one value if this is <typeparamref name="T"/>.</returns>
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
			fNone: () => $"None: {typeof(T).Name}",
			fSome: x => x?.ToString() switch
			{
				string value =>
					value,

				_ =>
					$"Some: {typeof(T).Name}"
			}
		);
}
