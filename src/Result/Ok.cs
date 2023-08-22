// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Diagnostics.CodeAnalysis;

namespace Monadic;

/// <summary>
/// 'OK' Result - wraps value to enable safe non-null returns (see <seealso cref="None"/>)
/// </summary>
/// <typeparam name="T">Result value type</typeparam>
public sealed record class Ok<T> : Result<T>, IRight<Exception, T>
{
	/// <summary>
	/// OK value - nullability will match the nullability of <typeparamref name="T"/>
	/// </summary>
	[MemberNotNull]
	public T Value { get; private init; }

	/// <summary>
	/// Internal creation only.
	/// </summary>
	/// <param name="value">OK value.</param>
	internal Ok([DisallowNull] T value) =>
		Value = value;
}
