// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Diagnostics.CodeAnalysis;

namespace Monadic;

public sealed record class Ok<T> : Result<T>, IRight<Exception, T>
{
	[MemberNotNull]
	public T Value { get; private init; }

	/// <summary>
	/// Internal creation only.
	/// </summary>
	/// <param name="value">OK value.</param>
	internal Ok(T value) =>
		Value = value;
}
