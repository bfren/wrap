// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Diagnostics.CodeAnalysis;

namespace Wrap;

/// <summary>
/// 'Some' Maybe - wraps value to enable safe non-null returns (see <seealso cref="None"/>).
/// </summary>
/// <typeparam name="T">Some value type.</typeparam>
public sealed record class Some<T> : Maybe<T>, IRight<None, T>
{
	/// <summary>
	/// Maybe value - nullability will match the nullability of <typeparamref name="T"/>.
	/// </summary>
	[MemberNotNull]
	public T Value { get; init; }

	/// <summary>
	/// Only allow internal creation by Wrap() function.
	/// </summary>
	/// <param name="value">Value to wrap</param>
	internal Some([DisallowNull] T value) =>
		Value = value switch
		{
			T =>
				value,

			_ =>
				throw new ArgumentNullException(nameof(value))
		};
}
