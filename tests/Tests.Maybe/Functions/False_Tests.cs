// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Functions_Tests;

public class False_Tests
{
	[Fact]
	public void Returns_Some_With_False_Value()
	{
		// Arrange
		var expected = false;

		// Act
		var result = M.False;

		// Assert
		result.AssertSome(expected);
	}
}
