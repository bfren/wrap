// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Functions_Tests;

public class True_Tests
{
	[Fact]
	public void Returns_Ok_With_True_Value()
	{
		// Arrange
		var expected = true;

		// Act
		var result = R.True;

		// Assert
		result.AssertOk(expected);
	}
}
