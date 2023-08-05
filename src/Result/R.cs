// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Monadic;

/// <summary>
/// Pure functions for interacting with <see cref="Result{TOk}"/> objects.
/// </summary>
public static partial class R
{
	public delegate void ErrorLogger(string error);

	public delegate void ExceptionLogger(Exception exception);
}
