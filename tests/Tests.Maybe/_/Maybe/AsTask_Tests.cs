// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Maybe_Tests;

public class AsTask_Tests
{
	[Fact]
	public async Task Wraps_Object_In_Task()
	{
		// Arrange
		var value = M.Wrap(Rnd.Int);

		// Act
		var result = await value.AsTask();

		// Assert
		Assert.Equal(value, result);
	}
}
