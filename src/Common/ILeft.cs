// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Monadic;

public interface ILeft<out TLeft, out TRight> : IEither<TLeft, TRight>
{
	TLeft Value { get; }
}
