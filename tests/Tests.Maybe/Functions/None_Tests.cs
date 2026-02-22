// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Functions_Tests;

public class None_Tests
{
	[Fact]
	public void Returns_Same_Value()
	{
		// Arrange
		var expected = M.None;

		// Act
		var result = M.None;

		// Assert
		Assert.Equal(expected, result);
	}
}
