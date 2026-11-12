// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

public readonly partial struct Maybe<T>
{
	#region Match

	/// <inheritdoc/>
	TReturn IEither<None, T>.Match<TReturn>(Func<None, TReturn> fLeft, Func<T, TReturn> fRight) =>
		M.Match(this, fNone: () => fLeft(default), fSome: fRight);

	#endregion

	#region TryGetValue

	/// <summary>
	/// Get the <see cref="Some{T}"/> case, if this object holds one.
	/// </summary>
	/// <remarks>
	/// Called by the compiler for <c>maybe is Some&lt;T&gt;</c> patterns, so matching does not allocate.
	/// </remarks>
	/// <param name="some">The <see cref="Some{T}"/> case, or a <see cref="Some{T}"/> holding <c>default</c> if this is <see cref="None"/>.</param>
	/// <returns><see langword="true"/> if this is a <see cref="Some{T}"/>.</returns>
	public bool TryGetValue(out Some<T> some)
	{
		some = new(value!);
		return isSome;
	}

	/// <summary>
	/// Get the <see cref="None"/> case, if this object holds it.
	/// </summary>
	/// <remarks>
	/// Called by the compiler for <c>maybe is None</c> patterns, so matching does not allocate.
	/// </remarks>
	/// <param name="none">Always the default <see cref="None"/> value.</param>
	/// <returns><see langword="true"/> if this is <see cref="None"/>.</returns>
	public bool TryGetValue(out None none)
	{
		none = default;
		return !isSome;
	}

	#endregion

	#region Unwrap

	/// <summary>
	/// Return the value if this is a <see cref="Some{T}"/>, otherwise the value returned by <paramref name="getValue"/>.
	/// </summary>
	/// <param name="getValue">Returns the value to use if this is <see cref="None"/>.</param>
	/// <returns>The <see cref="Some{T}"/> value, or the value returned by <paramref name="getValue"/>.</returns>
	public T Unwrap(Func<T> getValue) =>
		Unwrap(_ => getValue());

	/// <inheritdoc cref="Unwrap(Func{T})"/>
	public T Unwrap(Func<None, T> getValue) =>
		M.Match(this,
			fNone: () => getValue(default),
			fSome: x => x
		);

	#endregion

	#region ToString

	/// <summary>
	/// Convert the current object to a string.
	/// </summary>
	/// <returns>
	/// If this is a <see cref="Some{T}"/>, the value's string (or <c>Some: {type name}</c> when that is <see langword="null"/>).
	/// If this is <see cref="None"/>, <c>None: {type name}</c>.
	/// </returns>
	public override string ToString() =>
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

	#endregion
}
