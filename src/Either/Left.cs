// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <inheritdoc cref="ILeft{TLeft, TRight}"/>
public sealed record class Left<TLeft, TRight> : Either<TLeft, TRight>, ILeft<TLeft, TRight>
{
	/// <inheritdoc/>
	public TLeft Value { get; init; }

	/// <summary>
	/// Internal creation only.
	/// </summary>
	/// <seealso cref="E.WrapLeft{TLeft, TRight}(TLeft)"/>
	/// <param name="value">Left (error / invalid) value</param>
	internal Left(TLeft value) =>
		Value = value;
}
