// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

/// <summary>
/// Pure functions for interacting with <see cref="Result{TOk}"/> objects.
/// </summary>
public static partial class R
{
	/// <summary>
	/// Handles exceptions when an operation fails - see <see cref="Try{T}(Func{T})"/>.
	/// </summary>
	/// <param name="e">Exception object.</param>
	/// <returns>Fail value.</returns>
	public delegate Fail ExceptionHandler(Exception e);

	/// <summary>
	/// Default exception handler.
	/// </summary>
	public static ExceptionHandler DefaultHandler =>
		Fail;
}
