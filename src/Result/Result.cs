// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wrap.Exceptions;

namespace Wrap;

/// <summary>
/// Result monad.
/// </summary>
/// <typeparam name="T">Ok value type.</typeparam>
public abstract partial record class Result<T> : IEither<Result<T>, FailureValue, T>
{
	/// <summary>
	/// Returns true if this object is a <see cref="Wrap.Failure"/>.
	/// </summary>
	public bool Failed =>
		!IsOk;

	/// <summary>
	/// Returns true if this object is a <see cref="Ok{T}"/>.
	/// </summary>
	public bool IsOk =>
		this is Ok<T>;

	/// <inheritdoc/>
	public Task<Result<T>> AsTask() =>
		Task.FromResult(this);

	/// <inheritdoc/>
	public T Unwrap(Func<FailureValue, T> getValue) =>
		R.Match(this,
			fail: f => getValue(f),
			ok: x => x
		);

	/// <summary>
	/// Unwrap the value contained in this object - throws an exception if the result is <see cref="FailureImpl"/>.
	/// </summary>
	/// <returns>Result value or throws a <see cref="FailureException"/>.</returns>
	/// <exception cref="FailureException"></exception>
	public T Unwrap() =>
		R.Match(this,
			fail: R.ThrowFailure<T>,
			ok: x => x
		);

	/// <summary>
	/// Unwrap the value contained in this object - uses failure handler if the result is <see cref="FailureImpl"/>.
	/// </summary>
	/// <returns>Result value or throws a <see cref="FailureException"/>.</returns>
	/// <exception cref="FailureException"></exception>
	public T Unwrap(Action<FailureValue> ifFailed, Func<T> getValue) =>
		R.Match(this,
			fail: f => { ifFailed(f); return getValue(); },
			ok: x => x
		);

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
			fail: x => x.Message,
			ok: x => x?.ToString() switch
			{
				string value =>
					value,

				_ =>
					$"OK: {typeof(T)}"
			}
		);
}
