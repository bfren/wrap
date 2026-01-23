// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap.Testing;

public static partial class FailValueExtensions
{
	/// <summary>
	/// Assert that <paramref name="this"/> contains the specified message.
	/// </summary>
	/// <param name="this">FailValue object.</param>
	/// <param name="ex">Expected Exception.</param>
	/// <returns>Original FailValue.</returns>
	public static FailValue AssertException(this FailValue @this, Exception ex)
	{
		// Assert correct Exception
		Assert.Equal(ex, @this.Exception);

		// Return value
		return @this;
	}
}
