// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Globalization;

namespace Wrap.Result_Tests;

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
			var result = R.Wrap(1.5m).ToString();
			var ok = new Ok<decimal>(1.5m).ToString();

			// Assert
			Assert.Equal("1.5", result);
			Assert.Equal("Ok { Value = 1.5 }", ok);
		}
		finally
		{
			CultureInfo.CurrentCulture = current;
		}
	}
}
