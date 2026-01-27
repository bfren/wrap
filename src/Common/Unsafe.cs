// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Unsafe wrapper to enable unsafe (i.e. may result in null values) functions.
/// </summary>
/// <typeparam name="TEither">Either implementation type.</typeparam>
/// <typeparam name="TLeft">Left (error / invalid) value type.</typeparam>
/// <typeparam name="TRight">Right (correct / valid) value type.</typeparam>
public readonly record struct Unsafe<TEither, TLeft, TRight> : IUnion<TEither>
	where TEither : IEither<TEither, TLeft, TRight>
{
	/// <summary>
	/// Wrapped <typeparamref name="TEither"/> object.
	/// </summary>
	public TEither Value { get; init; }

	/// <summary>
	/// Internal creation only.
	/// </summary>
	/// <param name="value"><typeparamref name="TEither"/> object to wrap.</param>
	internal Unsafe(TEither value) =>
		Value = value;
}
