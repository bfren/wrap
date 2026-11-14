// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap.Testing;

public static partial class FailureExtensions
{
	/// <summary>
	/// Assert that <paramref name="this"/> holds the expected exception.
	/// </summary>
	/// <param name="this">Failure object.</param>
	/// <param name="ex">Expected exception.</param>
	/// <returns><paramref name="this"/>, so assertions can be chained.</returns>
	public static Failure AssertException(this Failure @this, Exception ex)
	{
		// Assert correct Exception
		Assert.Equal(ex, @this.Exception);

		// Return value
		return @this;
	}
}
