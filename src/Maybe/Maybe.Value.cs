// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

public readonly partial struct Maybe<T>
{
	#region IEither

	/// <inheritdoc/>
	bool IEither<None, T>.TryGetLeft(out None left)
	{
		left = default;
		return !isSome;
	}

	/// <inheritdoc/>
	bool IEither<None, T>.TryGetRight(out T right)
	{
		right = value!;
		return isSome;
	}

	#endregion

	#region TryGetValue

	/// <summary>
	/// Get the <see cref="Some{T}"/> case, if this object holds one.
	/// </summary>
	/// <remarks>
	/// Called by the compiler for <c>maybe is Some&lt;T&gt;</c> patterns and <see langword="switch"/>, so matching does not allocate.
	/// </remarks>
	/// <param name="some">The <see cref="Some{T}"/> case, or a <see cref="Some{T}"/> holding <see langword="default"/> if this is <see cref="None"/>.</param>
	/// <returns><see langword="true"/> if this is a <see cref="Some{T}"/>, otherwise <see langword="false"/>.</returns>
	public bool TryGetValue(out Some<T> some)
	{
		some = new(value!);
		return isSome;
	}

	/// <summary>
	/// Get the <see cref="None"/> case, if this object holds it.
	/// </summary>
	/// <remarks>
	/// Called by the compiler for <c>maybe is None</c> patterns and <see langword="switch"/>, so matching does not allocate.
	/// </remarks>
	/// <param name="none">Always the default <see cref="None"/> value.</param>
	/// <returns><see langword="true"/> if this is <see cref="None"/>, otherwise <see langword="false"/>.</returns>
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
		this switch
		{
			Some<T> x =>
				x.Value,

			None =>
				getValue()
		};

	/// <inheritdoc cref="Unwrap(Func{T})"/>
	public T Unwrap(Func<None, T> getValue) =>
		this switch
		{
			Some<T> x =>
				x.Value,

			None =>
				getValue(default)
		};

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
