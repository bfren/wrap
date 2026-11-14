// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;

namespace Wrap;

public readonly partial struct Result<T>
{

	/// <summary>
	/// Compare this object with <paramref name="obj"/> - see <see cref="Equals(Result{T})"/>.
	/// </summary>
	/// <param name="obj">Object to compare.</param>
	/// <returns><see langword="true"/> if <paramref name="obj"/> is a <see cref="Result{T}"/> equal to this object.</returns>
	public override bool Equals(object? obj) =>
		obj is Result<T> m && Equals(m);

	/// <summary>
	/// Compare this object with <paramref name="other"/>.
	/// </summary>
	/// <remarks>
	/// Failures are compared with <see cref="Failure.Equals(Failure)"/>.
	/// </remarks>
	/// <param name="other">Result to compare.</param>
	/// <returns>
	/// <see langword="true"/> if both are <see cref="Ok{T}"/> with equal values, or both are equal <see cref="Failure"/> values.
	/// </returns>
	public bool Equals(Result<T> other) =>
		other switch
		{
			Ok<T> x when this is Ok<T> y =>
				EqualityComparer<T>.Default.Equals(x.Value, y.Value),

			Failure x when this is Failure y =>
				EqualityComparer<Failure>.Default.Equals(x, y),

			_ =>
				false
		};

	/// <summary>
	/// Get the hash code of the <see cref="Ok{T}"/> value, or zero if this is a <see cref="Failure"/>.
	/// </summary>
	/// <returns>Hash code.</returns>
	public override int GetHashCode() =>
		this is Ok<T> s ? s.Value?.GetHashCode() ?? 0 : 0;

	/// <summary>
	/// Compare two <see cref="Result{T}"/> objects - see <see cref="Equals(Result{T})"/>.
	/// </summary>
	/// <param name="l">Result.</param>
	/// <param name="r">Result.</param>
	/// <returns><see langword="true"/> if both are <see cref="Ok{T}"/> with equal values, or both are equal <see cref="Failure"/> values.</returns>
	public static bool operator ==(Result<T> l, Result<T> r) =>
		EqualityComparer<Result<T>>.Default.Equals(l, r);

	/// <summary>
	/// Compare two <see cref="Result{T}"/> objects - see <see cref="Equals(Result{T})"/>.
	/// </summary>
	/// <param name="l">Result.</param>
	/// <param name="r">Result.</param>
	/// <returns><see langword="true"/> unless both are <see cref="Ok{T}"/> with equal values, or both are equal <see cref="Failure"/> values.</returns>
	public static bool operator !=(Result<T> l, Result<T> r) =>
		!EqualityComparer<Result<T>>.Default.Equals(l, r);

	/// <summary>
	/// Compare a <see cref="Result{T}"/> with a <typeparamref name="T"/> value.
	/// </summary>
	/// <param name="l">Result.</param>
	/// <param name="r">Value.</param>
	/// <returns><see langword="true"/> if <paramref name="l"/> is an <see cref="Ok{T}"/> and its value equals <paramref name="r"/>.</returns>
	public static bool operator ==(Result<T> l, T r) =>
		l is Ok<T> s && EqualityComparer<T>.Default.Equals(s.Value, r);

	/// <summary>
	/// Compare a <see cref="Result{T}"/> with a <typeparamref name="T"/> value.
	/// </summary>
	/// <param name="l">Result.</param>
	/// <param name="r">Value.</param>
	/// <returns><see langword="true"/> if <paramref name="l"/> is a <see cref="Failure"/>, or an <see cref="Ok{T}"/> whose value does not equal <paramref name="r"/>.</returns>
	public static bool operator !=(Result<T> l, T r) =>
		!(l is Ok<T> s && EqualityComparer<T>.Default.Equals(s.Value, r));

	/// <summary>
	/// Compare a <typeparamref name="T"/> value with a <see cref="Result{T}"/>.
	/// </summary>
	/// <param name="l">Value.</param>
	/// <param name="r">Result.</param>
	/// <returns><see langword="true"/> if <paramref name="r"/> is an <see cref="Ok{T}"/> and its value equals <paramref name="l"/>.</returns>
	public static bool operator ==(T l, Result<T> r) =>
		r is Ok<T> s && EqualityComparer<T>.Default.Equals(s.Value, l);

	/// <summary>
	/// Compare a <typeparamref name="T"/> value with a <see cref="Result{T}"/>.
	/// </summary>
	/// <param name="l">Value.</param>
	/// <param name="r">Result.</param>
	/// <returns><see langword="true"/> if <paramref name="r"/> is a <see cref="Failure"/>, or an <see cref="Ok{T}"/> whose value does not equal <paramref name="l"/>.</returns>
	public static bool operator !=(T l, Result<T> r) =>
		!(r is Ok<T> s && EqualityComparer<T>.Default.Equals(s.Value, l));
}
