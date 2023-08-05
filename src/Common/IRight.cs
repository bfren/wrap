// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Monadic;

public interface IRight<out TLeft, out TRight> : IEither<TLeft, TRight>
{
	TRight Value { get; }
}
