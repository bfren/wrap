// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Testing;

public static partial class FailureExtensions
{
	/// <summary>
	/// Assert that <paramref name="this"/> has the expected message and, if given, arguments.
	/// </summary>
	/// <param name="this">Failure object.</param>
	/// <param name="message">Expected failure message.</param>
	/// <param name="args">Optional arguments to fill in failure message values - not checked when empty.</param>
	/// <returns>Original Failure.</returns>
	public static Failure AssertMessage(this Failure @this, string message, params object?[] args)
	{
		// Assert correct message
		Assert.Equal(message, @this.Message);

		// Assert args if provided
		if (args.Length > 0)
		{
			Assert.Equal(args, @this.Args);
		}

		// Return value
		return @this;
	}
}
