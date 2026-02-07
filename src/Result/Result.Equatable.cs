// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

public abstract partial record class Result<T>
{
	/// <inheritdoc/>
	public virtual bool Equals(Result<T>? other) =>
		this switch
		{
			Ok<T> x when other is Ok<T> y =>
				Equals(x.Value, y.Value),

			FailureImpl when other is FailureImpl =>
				true,

			_ =>
				false
		};

	/// <inheritdoc/>
	public override int GetHashCode() =>
		R.Match(this,
			fFail: f => f.GetHashCode(),
			fOk: x => x.GetHashCode()
		);

	/// <summary>
	/// Compare a Result type with a value type.
	/// <para>If <paramref name="l"/> is a <see cref="Ok{T}"/> the <see cref="Ok{T}.Value"/> will be compared to <paramref name="r"/>.</para>
	/// </summary>
	/// <param name="l">Result.</param>
	/// <param name="r">Value.</param>
	/// <returns>True if <paramref name="l"/> is <see cref="Ok{T}"/> and its value equals <paramref name="r"/>.</returns>
	public static bool operator ==(Result<T> l, T r) =>
		R.Match(l,
			fFail: _ => false,
			fOk: x => Equals(x, r)
		);

	/// <summary>
	/// Compare a Result type with a value type.
	/// <para>If <paramref name="l"/> is a <see cref="Ok{T}"/> the <see cref="Ok{T}.Value"/> will be compared to <paramref name="r"/>.</para>
	/// </summary>
	/// <param name="l">Result.</param>
	/// <param name="r">Value.</param>
	/// <returns>True if <paramref name="l"/> is <see cref="Ok{T}"/> and its value does not equal <paramref name="r"/>.</returns>
	public static bool operator !=(Result<T> l, T r) =>
		R.Match(l,
			fFail: _ => true,
			fOk: x => !Equals(x, r)
		);

	/// <summary>
	/// Compare a Result type with a value type.
	/// <para>If <paramref name="l"/> is a <see cref="Ok{T}"/> the <see cref="Ok{T}.Value"/> will be compared to <paramref name="r"/>.</para>
	/// </summary>
	/// <param name="l">Value.</param>
	/// <param name="r">Result.</param>
	/// <returns>True if <paramref name="r"/> is <see cref="Ok{T}"/> and its value equals <paramref name="l"/>.</returns>
	public static bool operator ==(T l, Result<T> r) =>
		R.Match(r,
			fFail: _ => false,
			fOk: x => Equals(x, l)
		);

	/// <summary>
	/// Compare a Result type with a value type.
	/// <para>If <paramref name="l"/> is a <see cref="Ok{T}"/> the <see cref="Ok{T}.Value"/> will be compared to <paramref name="r"/>.</para>
	/// </summary>
	/// <param name="l">Value.</param>
	/// <param name="r">Result.</param>
	/// <returns>True if <paramref name="r"/> is <see cref="Ok{T}"/> and its value does not equal <paramref name="l"/>.</returns>
	public static bool operator !=(T l, Result<T> r) =>
		R.Match(r,
			fFail: _ => true,
			fOk: x => !Equals(x, l)
		);
}
