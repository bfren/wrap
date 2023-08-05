// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Monadic;

public static partial class F
{
	public delegate void ErrorLogger(string error);

	public delegate void ExceptionLogger(Exception exception);
}
