// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.M_Tests;

public class IntegerNumberStyles_Tests
{
	[Fact]
	public void Returns_Integer_NumberStyles()
	{
		// Arrange

		// Act
		var result = M.IntegerNumberStyles;

		// Assert
		Assert.Equal(System.Globalization.NumberStyles.Integer, result);
	}
}
