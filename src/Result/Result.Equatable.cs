// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// result.
/// </summary>
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
		this is Ok<T> s ? s.Value?.GetHashCode() ?? 0 : 0;

	/// <summary>
	/// Compare a Result type with a value type.
	/// <para>If <paramref name="l"/> is a <see cref="Ok{T}"/> the <see cref="Ok{T}.Value"/> will be compared to <paramref name="r"/>.</para>
	/// </summary>
	/// <param name="l">Result.</param>
	/// <param name="r">Value.</param>
	/// <returns>True if <paramref name="l"/> is <see cref="Ok{T}"/> and its value equals <paramref name="r"/>.</returns>
	public static bool operator ==(Result<T> l, T r) =>
		l is Ok<T> s && Equals(s.Value, r);

	/// <summary>
	/// Compare a Result type with a value type.
	/// <para>If <paramref name="l"/> is a <see cref="Ok{T}"/> the <see cref="Ok{T}.Value"/> will be compared to <paramref name="r"/>.</para>
	/// </summary>
	/// <param name="l">Result.</param>
	/// <param name="r">Value.</param>
	/// <returns>True if <paramref name="l"/> is <see cref="Ok{T}"/> and its value does not equal <paramref name="r"/>.</returns>
	public static bool operator !=(Result<T> l, T r) =>
		!(l is Ok<T> s && Equals(s.Value, r));

	/// <summary>
	/// Compare a Result type with a value type.
	/// <para>If <paramref name="l"/> is a <see cref="Ok{T}"/> the <see cref="Ok{T}.Value"/> will be compared to <paramref name="r"/>.</para>
	/// </summary>
	/// <param name="l">Value.</param>
	/// <param name="r">Result.</param>
	/// <returns>True if <paramref name="r"/> is <see cref="Ok{T}"/> and its value equals <paramref name="l"/>.</returns>
	public static bool operator ==(T l, Result<T> r) =>
		r is Ok<T> s && Equals(s.Value, l);

	/// <summary>
	/// Compare a Result type with a value type.
	/// <para>If <paramref name="l"/> is a <see cref="Ok{T}"/> the <see cref="Ok{T}.Value"/> will be compared to <paramref name="r"/>.</para>
	/// </summary>
	/// <param name="l">Value.</param>
	/// <param name="r">Result.</param>
	/// <returns>True if <paramref name="r"/> is <see cref="Ok{T}"/> and its value does not equal <paramref name="l"/>.</returns>
	public static bool operator !=(T l, Result<T> r) =>
		!(r is Ok<T> s && Equals(s.Value, l));
}
