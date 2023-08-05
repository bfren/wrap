// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Diagnostics.CodeAnalysis;

namespace Monadic;

/// <summary>
/// 'Some' Maybe - wraps value to enable safe non-null returns (see <seealso cref="None{T}"/>)
/// </summary>
/// <typeparam name="T">Maybe value type</typeparam>
public sealed record class Some<T> : Maybe<T>, IRight<None, T>
{
	/// <summary>
	/// Maybe value - nullability will match the nullability of <typeparamref name="T"/>
	/// </summary>
	[MemberNotNull]
	public T Value { get; private init; }

	/// <summary>
	/// Only allow internal creation by Some() functions and implicit operator
	/// </summary>
	/// <param name="value">Value to wrap</param>
	internal Some(T value) =>
		Value = value;
}
