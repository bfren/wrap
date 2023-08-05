// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;

namespace Monadic;

public interface IEither<out TLeft, out TRight>
{
	IEnumerator<TRight> GetEnumerator();
}
