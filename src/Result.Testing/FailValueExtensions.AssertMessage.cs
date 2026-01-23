// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Testing;

public static partial class FailValueExtensions
{
	/// <summary>
	/// Assert that <paramref name="this"/> contains the specified message.
	/// </summary>
	/// <param name="this">FailValue object.</param>
	/// <param name="message">Expected failure message.</param>
	/// <param name="args">Optional arguments to fill in failure message values.</param>
	public static FailValue AssertMessage(this FailValue @this, string message, params object?[] args)
	{
		// Assert correct message
		Assert.Equal(message, @this.Message);

		// Assert args if provided
		if (args is not null)
		{
			Assert.Equal(args, @this.Args);
		}

		// Return value
		return @this;
	}
}
