// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Globalization;

namespace Wrap.Monad_Tests;

public class ToString_Culture_Tests
{
	[Fact]
	public void Uses_DefaultCulture_Not_CurrentCulture()
	{
		// Arrange
		var current = CultureInfo.CurrentCulture;
		CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");

		try
		{
			// Act
			var result = new Price().ToString();

			// Assert
			Assert.Equal("Price { Value = 1.5 }", result);
		}
		finally
		{
			CultureInfo.CurrentCulture = current;
		}
	}

	public sealed record class Price() : Monad<Price, decimal>(1.5m);
}
