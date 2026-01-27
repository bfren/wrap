// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <inheritdoc cref="IRight{TLeft, TRight}"/>
public sealed record class Right<TLeft, TRight> : Either<TLeft, TRight>, IRight<TLeft, TRight>
{
	/// <inheritdoc/>
	public TRight Value { get; init; }

	/// <summary>
	/// Internal creation only.
	/// </summary>
	/// <seealso cref="E.WrapRight{TLeft, TRight}(TRight)"/>
	/// <param name="value">Right (correct / valid) value</param>
	internal Right(TRight value) =>
		Value = value;
}
