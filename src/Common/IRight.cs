// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Either monad - right value.
/// </summary>
/// <typeparam name="TLeft">Left (error / invalid) value type.</typeparam>
/// <typeparam name="TRight">Right (correct / valid) value type.</typeparam>
public interface IRight<TLeft, TRight> : IEither<TLeft, TRight>, IUnion<TRight> { }
