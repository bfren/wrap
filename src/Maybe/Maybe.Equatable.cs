// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Wrap;

public readonly partial struct Maybe<T>
{
	/// <summary>
	/// Compare this object with <paramref name="obj"/> - see <see cref="Equals(Maybe{T})"/>.
	/// </summary>
	/// <param name="obj">Object to compare.</param>
	/// <returns><see langword="true"/> if <paramref name="obj"/> is a <see cref="Maybe{T}"/> equal to this object.</returns>
	public override bool Equals([NotNullWhen(true)] object? obj) =>
		obj is Maybe<T> m && Equals(m);

	/// <summary>
	/// Compare this object with <paramref name="other"/>.
	/// </summary>
	/// <param name="other">Maybe to compare.</param>
	/// <returns>
	/// <see langword="true"/> if both are <see cref="Some{T}"/> with equal values, or both are <see cref="None"/>.
	/// </returns>
	public bool Equals(Maybe<T> other) =>
		other switch
		{
			Some<T> s =>
				this is Some<T> s2 && EqualityComparer<T>.Default.Equals(s.Value, s2.Value),

			None =>
				this is None,

			_ =>
				false
		};

	/// <summary>
	/// Get the hash code of the <see cref="Some{T}"/> value, or zero if this is <see cref="None"/>.
	/// </summary>
	/// <returns>Hash code.</returns>
	public override int GetHashCode() =>
		this is Some<T> s ? s.Value?.GetHashCode() ?? 0 : 0;

	/// <summary>
	/// Compare two <see cref="Maybe{T}"/> objects - see <see cref="Equals(Maybe{T})"/>.
	/// </summary>
	/// <param name="l">Maybe.</param>
	/// <param name="r">Maybe.</param>
	/// <returns><see langword="true"/> if both are <see cref="Some{T}"/> with equal values, or both are <see cref="None"/>.</returns>
	public static bool operator ==(Maybe<T> l, Maybe<T> r) =>
		EqualityComparer<Maybe<T>>.Default.Equals(l, r);

	/// <summary>
	/// Compare two <see cref="Maybe{T}"/> objects - see <see cref="Equals(Maybe{T})"/>.
	/// </summary>
	/// <param name="l">Maybe.</param>
	/// <param name="r">Maybe.</param>
	/// <returns><see langword="true"/> unless both are <see cref="Some{T}"/> with equal values, or both are <see cref="None"/>.</returns>
	public static bool operator !=(Maybe<T> l, Maybe<T> r) =>
		!EqualityComparer<Maybe<T>>.Default.Equals(l, r);

	/// <summary>
	/// Compare a <see cref="Maybe{T}"/> with a <typeparamref name="T"/> value.
	/// </summary>
	/// <param name="l">Maybe.</param>
	/// <param name="r">Value.</param>
	/// <returns><see langword="true"/> if <paramref name="l"/> is a <see cref="Some{T}"/> and its value equals <paramref name="r"/>.</returns>
	public static bool operator ==(Maybe<T> l, T r) =>
		l is Some<T> s && EqualityComparer<T>.Default.Equals(s.Value, r);

	/// <summary>
	/// Compare a <see cref="Maybe{T}"/> with a <typeparamref name="T"/> value.
	/// </summary>
	/// <param name="l">Maybe.</param>
	/// <param name="r">Value.</param>
	/// <returns><see langword="true"/> if <paramref name="l"/> is <see cref="None"/>, or a <see cref="Some{T}"/> whose value does not equal <paramref name="r"/>.</returns>
	public static bool operator !=(Maybe<T> l, T r) =>
		!(l is Some<T> s && EqualityComparer<T>.Default.Equals(s.Value, r));

	/// <summary>
	/// Compare a <typeparamref name="T"/> value with a <see cref="Maybe{T}"/>.
	/// </summary>
	/// <param name="l">Value.</param>
	/// <param name="r">Maybe.</param>
	/// <returns><see langword="true"/> if <paramref name="r"/> is a <see cref="Some{T}"/> and its value equals <paramref name="l"/>.</returns>
	public static bool operator ==(T l, Maybe<T> r) =>
		r is Some<T> s && EqualityComparer<T>.Default.Equals(s.Value, l);

	/// <summary>
	/// Compare a <typeparamref name="T"/> value with a <see cref="Maybe{T}"/>.
	/// </summary>
	/// <param name="l">Value.</param>
	/// <param name="r">Maybe.</param>
	/// <returns><see langword="true"/> if <paramref name="r"/> is <see cref="None"/>, or a <see cref="Some{T}"/> whose value does not equal <paramref name="l"/>.</returns>
	public static bool operator !=(T l, Maybe<T> r) =>
		!(r is Some<T> s && EqualityComparer<T>.Default.Equals(s.Value, l));
}
