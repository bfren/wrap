// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Globalization;

namespace Wrap.M_Tests;

public class FloatNumberStyles_Tests
{
	[Fact]
	public void Returns_Number_NumberStyles()
	{
		// Arrange

		// Act
		var result = M.FloatNumberStyles;

		// Assert
		Assert.Equal(NumberStyles.Float | NumberStyles.AllowThousands, result);
	}
}
