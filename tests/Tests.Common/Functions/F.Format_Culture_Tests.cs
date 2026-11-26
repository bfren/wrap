// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Globalization;

namespace Wrap.F_Tests;

public class Format_Culture_Tests
{
	[Theory]
	[InlineData("{0}", "1.5")]
	[InlineData("{0:0.00}", "1.50")]
	public void Single_Value__Uses_DefaultCulture_Not_CurrentCulture(string format, string expected)
	{
		// Arrange
		var current = CultureInfo.CurrentCulture;
		CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");

		try
		{
			// Act
			var result = F.Format(format, 1.5m);

			// Assert
			Assert.Equal(expected, result);
		}
		finally
		{
			CultureInfo.CurrentCulture = current;
		}
	}
}
