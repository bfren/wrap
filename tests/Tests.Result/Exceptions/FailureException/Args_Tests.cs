// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Exceptions.FailureException_Tests;

public class Args_Tests
{
	[Fact]
	public void Returns_Failure_Args()
	{
		// Arrange
		var value = FailGen.Value;

		// Act
		var result = new FailureException(value);

		// Assert
		Assert.Equal(value.Args, result.Args);
	}
}
