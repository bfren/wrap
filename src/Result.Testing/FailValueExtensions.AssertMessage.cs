// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Globalization;

namespace Wrap.Testing;

public static partial class FailValueExtensions
{
	/// <summary>
	/// Assert that <paramref name="this"/> contains the specified message.
	/// </summary>
	/// <param name="this">FailValue object.</param>
	/// <param name="message">Expected failure message.</param>
	/// <param name="args">Optional arguments to fill in failure message values.</param>
	public static void AssertMessage(this Generator @this, string message, params object?[] args) =>
		Assert.Equal(string.Format(CultureInfo.InvariantCulture, message, args), @this.Message);
}
