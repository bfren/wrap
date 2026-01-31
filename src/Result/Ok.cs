// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Diagnostics.CodeAnalysis;

namespace Wrap;

/// <summary>
/// 'OK' Result - wraps value to enable safe non-null returns (see <seealso cref="Failure"/>).
/// </summary>
/// <typeparam name="T">Ok value type.</typeparam>
public sealed record class Ok<T> : Result<T>, IRight<FailureValue, T>
{
	/// <summary>
	/// OK value - Value is never null.
	/// </summary>
	[MemberNotNull]
	public T Value { get; init; }

	/// <summary>
	/// Internal creation only.
	/// </summary>
	/// <param name="value">OK value.</param>
	internal Ok([DisallowNull] T value) =>
		Value = value switch
		{
			T =>
				value,

			_ =>
				throw new ArgumentNullException(nameof(value))
		};
}
