// Monads: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Monads;

/// <summary>
/// Either monad - left value.
/// </summary>
/// <typeparam name="TLeft">Left (error / invalid) value type.</typeparam>
/// <typeparam name="TRight">Right (correct / valid) value type.</typeparam>
public interface ILeft<out TLeft, out TRight> : IEither<TLeft, TRight>
{
	/// <summary>
	/// Left (error / invalid) value.
	/// </summary>
	TLeft Value { get; }
}
