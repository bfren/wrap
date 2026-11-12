// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Functions_Tests;

public class Wrap_Tests
{
	[Fact]
	public void WrapLeft__Returns_Left_With_Value()
	{
		// Arrange
		var value = Rnd.Str;

		// Act
		var result = E.WrapLeft<string, int>(value);

		// Assert
		var left = Assert.IsType<Left<string>>(result.Value);
		Assert.Equal(value, left.Value);
	}

	[Fact]
	public void WrapRight__Returns_Right_With_Value()
	{
		// Arrange
		var value = Rnd.Int;

		// Act
		var result = E.WrapRight<string, int>(value);

		// Assert
		var right = Assert.IsType<Right<int>>(result.Value);
		Assert.Equal(value, right.Value);
	}
}
