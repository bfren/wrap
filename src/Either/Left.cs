// Monads: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Diagnostics.CodeAnalysis;

namespace Monads;

/// <inheritdoc cref="ILeft{TLeft, TRight}"/>
public sealed record class Left<TLeft, TRight> : Either<TLeft, TRight>, ILeft<TLeft, TRight>
{
	/// <inheritdoc/>
	[MemberNotNull]
	public TLeft Value { get; private init; }

	/// <summary>
	/// Internal creation only.
	/// </summary>
	/// <seealso cref="E.Left{TLeft, TRight}(TLeft)"/>
	/// <param name="value">Left (error / invalid) value</param>
	public Left(TLeft value) =>
		Value = value;
}
