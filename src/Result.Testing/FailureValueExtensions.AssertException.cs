// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap.Testing;

public static partial class FailureValueExtensions
{
	/// <summary>
	/// Assert that <paramref name="this"/> contains the specified message.
	/// </summary>
	/// <param name="this">FailureValue object.</param>
	/// <param name="ex">Expected Exception.</param>
	/// <returns>Original FailureValue.</returns>
	public static FailureValue AssertException(this FailureValue @this, Exception ex)
	{
		// Assert correct Exception
		Assert.Equal(ex, @this.Exception);

		// Return value
		return @this;
	}
}
