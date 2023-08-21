// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Diagnostics.CodeAnalysis;

namespace Monadic;

/// <inheritdoc cref="IRight{TLeft, TRight}"/>
public sealed record class Right<TLeft, TRight> : Either<TLeft, TRight>, IRight<TLeft, TRight>
{
	/// <inheritdoc/>
	[MemberNotNull]
	public TRight Value { get; private init; }

	/// <summary>
	/// Internal creation only.
	/// </summary>
	/// <seealso cref="E.Right{TLeft, TRight}(TRight)"/>
	/// <param name="value">Right (correct / valid) value</param>
	internal Right(TRight value) =>
		Value = value;
}
