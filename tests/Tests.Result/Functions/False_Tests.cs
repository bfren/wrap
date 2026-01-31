// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Functions_Tests;

public class False_Tests
{
	[Fact]
	public void Returns_Ok_With_False_Value()
	{
		// Arrange
		var expected = false;

		// Act
		var result = R.False;

		// Assert
		result.AssertOk(expected);
	}
}
