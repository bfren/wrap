// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Functions_Tests;

public class Ok_Tests
{
	[Fact]
	public void Returns_Ok_Result_With_True_Value()
	{
		// Arrange

		// Act
		var result = R.Ok();

		// Assert
		result.AssertOk(true);
	}
}
