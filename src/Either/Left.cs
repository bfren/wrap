// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Diagnostics.CodeAnalysis;

namespace Monadic;

public sealed record class Left<TLeft, TRight> : Either<TLeft, TRight>, ILeft<TLeft, TRight>
{
	[MemberNotNull]
	public TLeft Value { get; private init; }

	public Left(TLeft value) =>
		Value = value;
}
