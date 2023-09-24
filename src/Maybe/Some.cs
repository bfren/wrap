// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Diagnostics.CodeAnalysis;

namespace Wrap;

/// <summary>
/// 'Some' Maybe - wraps value to enable safe non-null returns (see <seealso cref="None"/>)
/// </summary>
/// <typeparam name="T">Some value type</typeparam>
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
	internal Some([DisallowNull] T value) =>
		Value = value;
}
