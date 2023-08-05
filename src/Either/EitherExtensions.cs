// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Monadic.Exceptions;

namespace Monadic;

public static partial class EitherExtensions
{
	public delegate InvalidEitherTypeException InvalidType<TLeft, TRight>(IEither<TLeft, TRight> either);
}
