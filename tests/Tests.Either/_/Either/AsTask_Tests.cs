// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Either_Tests;

public class AsTask_Tests
{
	[Fact]
	public async Task Returns_Completed_Task_With_Same_Value()
	{
		// Arrange
		Either<string, int> either = new Right<int>(Rnd.Int);

		// Act
		var result = await either.AsTask();

		// Assert
		Assert.Equal(either, result);
	}
}
