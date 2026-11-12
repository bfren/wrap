// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Wrap;

public readonly partial struct Either<TLeft, TRight>
{
	/// <summary>
	/// Compare this object with <paramref name="obj"/> - see <see cref="Equals(Either{TLeft, TRight})"/>.
	/// </summary>
	/// <param name="obj">Object to compare.</param>
	/// <returns><see langword="true"/> if <paramref name="obj"/> is an <see cref="Either{TLeft, TRight}"/> equal to this object.</returns>
	public override bool Equals([NotNullWhen(true)] object? obj) =>
		obj is Either<TLeft, TRight> e && Equals(e);

	/// <summary>
	/// Compare this object with <paramref name="other"/>.
	/// </summary>
	/// <param name="other">Either to compare.</param>
	/// <returns>
	/// <see langword="true"/> if both are <see cref="Left{T}"/> with equal values, or both are <see cref="Right{T}"/> with equal values.
	/// </returns>
	public bool Equals(Either<TLeft, TRight> other) =>
		other switch
		{
			Left<TLeft> x when this is Left<TLeft> y =>
				EqualityComparer<TLeft>.Default.Equals(x.Value, y.Value),

			Right<TRight> x when this is Right<TRight> y =>
				EqualityComparer<TRight>.Default.Equals(x.Value, y.Value),

			_ =>
				false
		};

	/// <summary>
	/// Get the hash code of the current case and its value.
	/// </summary>
	/// <returns>Hash code.</returns>
	public override int GetHashCode() =>
		this switch
		{
			Left<TLeft> l =>
				HashCode.Combine(false, l.Value),

			Right<TRight> r =>
				HashCode.Combine(true, r.Value)
		};

	/// <summary>
	/// Compare two <see cref="Either{TLeft, TRight}"/> objects - see <see cref="Equals(Either{TLeft, TRight})"/>.
	/// </summary>
	/// <param name="l">Either.</param>
	/// <param name="r">Either.</param>
	/// <returns><see langword="true"/> if both hold the same case with equal values.</returns>
	public static bool operator ==(Either<TLeft, TRight> l, Either<TLeft, TRight> r) =>
		l.Equals(r);

	/// <summary>
	/// Compare two <see cref="Either{TLeft, TRight}"/> objects - see <see cref="Equals(Either{TLeft, TRight})"/>.
	/// </summary>
	/// <param name="l">Either.</param>
	/// <param name="r">Either.</param>
	/// <returns><see langword="true"/> if they hold different cases, or the same case with different values.</returns>
	public static bool operator !=(Either<TLeft, TRight> l, Either<TLeft, TRight> r) =>
		!l.Equals(r);
}
