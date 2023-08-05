// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Diagnostics.CodeAnalysis;

namespace Monadic;

public sealed record class Right<TLeft, TRight> : Either<TLeft, TRight>, IRight<TLeft, TRight>
{
	[MemberNotNull]
	public TRight Value { get; private init; }

	public Right(TRight value) =>
		Value = value;
}
