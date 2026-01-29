// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Result_Tests.FailureImpl_Tests;

public class Constructor_Tests
{
	[Fact]
	public void Sets_Value_To_NoneImpl()
	{
		// Arrange
		var value = FailGen.Value;

		// Act
		var result = new Result<int>.FailureImpl(value);

		// Assert
		Assert.Equal(value, result.Value);
	}
}
