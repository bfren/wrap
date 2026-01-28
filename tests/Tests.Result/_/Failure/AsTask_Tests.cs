// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Failure_Tests;

public class AsTask_Tests
{
	[Fact]
	public async Task Returns_Task_With_FailureValue()
	{
		// Arrange
		var failure = FailGen.Create();

		// Act
		var result = await failure.AsTask<int>();

		// Assert
		var f = result.AssertFailure();
		Assert.Equal(failure.Value, f);
	}
}
