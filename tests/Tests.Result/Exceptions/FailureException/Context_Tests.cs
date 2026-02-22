// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Exceptions.FailureException_Tests;

public class Context_Tests
{
	[Fact]
	public void Returns_Failure_Context()
	{
		// Arrange
		var value = FailGen.Value with { Context = Rnd.Str };

		// Act
		var result = new FailureException(value);

		// Assert
		Assert.Equal(value.Context, result.Context);
	}
}
