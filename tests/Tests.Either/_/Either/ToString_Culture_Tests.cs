// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Globalization;

namespace Wrap.Either_Tests;

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
			Either<decimal, int> left = new Left<decimal>(1.5m);
			Either<int, decimal> right = new Right<decimal>(2.5m);

			// Assert
			Assert.Equal("Left { Value = 1.5 }", left.ToString());
			Assert.Equal("Right { Value = 2.5 }", right.ToString());
		}
		finally
		{
			CultureInfo.CurrentCulture = current;
		}
	}
}
