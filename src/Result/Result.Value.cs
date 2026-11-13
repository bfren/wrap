// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

public readonly partial struct Result<T>
{
	#region IEither

	/// <inheritdoc/>
	/// <remarks>
	/// For an <see cref="Ok{T}"/>, and for <c>default(Result&lt;T&gt;)</c>, <paramref name="left"/> is a <see cref="Failure"/>
	/// with the message <c>Uninitialised Failure</c>, as with <see cref="TryGetValue(out Failure)"/>.
	/// </remarks>
	bool IEither<Failure, T>.TryGetLeft(out Failure left) =>
		TryGetValue(out left);

	/// <inheritdoc/>
	bool IEither<Failure, T>.TryGetRight(out T right)
	{
		right = value!;
		return isOk;
	}

	#endregion

	#region TryGetValue

	/// <summary>
	/// Get the <see cref="Ok{T}"/> case, if this object holds one.
	/// </summary>
	/// <remarks>
	/// Called by the compiler for <c>result is Ok&lt;T&gt;</c> patterns and <c>switch</c>, so matching does not allocate.
	/// </remarks>
	/// <param name="ok">The <see cref="Ok{T}"/> case, or an <see cref="Ok{T}"/> holding <c>default</c> if this is a <see cref="Failure"/>.</param>
	/// <returns><see langword="true"/> if this is an <see cref="Ok{T}"/>.</returns>
	public bool TryGetValue(out Ok<T> ok)
	{
		ok = new(value!);
		return isOk;
	}

	/// <summary>
	/// Get the <see cref="Failure"/> case, if this object holds one.
	/// </summary>
	/// <remarks>
	/// Called by the compiler for <c>result is Failure</c> patterns and <c>switch</c>, so matching does not allocate.
	/// For <c>default(Result&lt;T&gt;)</c> the failure has the message <c>Uninitialised Failure</c>.
	/// </remarks>
	/// <param name="failure">
	/// The <see cref="Failure"/> case, or a <see cref="Failure"/> with the message <c>Uninitialised Failure</c>
	/// if this is an <see cref="Ok{T}"/>.
	/// </param>
	/// <returns><see langword="true"/> if this is a <see cref="Failure"/>.</returns>
	public bool TryGetValue(out Failure failure)
	{
		failure = this.failure?.Value ?? Failure.Default;
		return !isOk;
	}

	#endregion

	#region Unwrap

	/// <summary>
	/// Return the value if this is an <see cref="Ok{T}"/>, otherwise the value returned by <paramref name="getValue"/>.
	/// </summary>
	/// <param name="getValue">Returns the value to use, given the <see cref="Failure"/>.</param>
	/// <returns>The <see cref="Ok{T}"/> value, or the value returned by <paramref name="getValue"/>.</returns>
	public T Unwrap(Func<Failure, T> getValue) =>
		R.Match(this,
			fFail: f => getValue(f),
			fOk: x => x
		);

	/// <summary>
	/// Return the value if this is an <see cref="Ok{T}"/>, otherwise run <paramref name="ifFailed"/>
	/// and return the value returned by <paramref name="getValue"/>.
	/// </summary>
	/// <param name="ifFailed">Action to run with the <see cref="Failure"/>, for example to log it.</param>
	/// <param name="getValue">Returns the value to use if this is a <see cref="Failure"/>.</param>
	/// <returns>The <see cref="Ok{T}"/> value, or the value returned by <paramref name="getValue"/>.</returns>
	public T Unwrap(Action<Failure> ifFailed, Func<T> getValue) =>
		Unwrap(f => { ifFailed(f); return getValue(); });

	#endregion

	#region ToString

	/// <summary>
	/// Convert the current object to a string.
	/// </summary>
	/// <returns>
	/// If this is an <see cref="Ok{T}"/>, the value's string (or <c>OK: {type name}</c> when that is <see langword="null"/>).
	/// If this is a <see cref="Failure"/>, its <see cref="Failure.Message"/> with the placeholders filled from <see cref="Failure.Args"/>.
	/// </returns>
	public override string ToString() =>
		R.Match(this,
			fFail: x => F.Format(x.Message, x.Args),
			fOk: x => x?.ToString() switch
			{
				string value =>
					value,

				_ =>
					$"OK: {typeof(T).Name}"
			}
		);

	#endregion
}
