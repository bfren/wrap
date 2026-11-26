// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Globalization;

namespace Wrap.Maybe_Tests;

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
			var maybe = M.Wrap(1.5m).ToString();
			var some = new Some<decimal>(1.5m).ToString();

			// Assert
			Assert.Equal("1.5", maybe);
			Assert.Equal("Some { Value = 1.5 }", some);
		}
		finally
		{
			CultureInfo.CurrentCulture = current;
		}
	}
}
