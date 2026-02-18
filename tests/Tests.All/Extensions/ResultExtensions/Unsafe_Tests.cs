// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class Unsafe_Tests
{
	[Fact]
	public void Returns_Unsafe_Wrapping_Result()
	{
		// Arrange
		var value = Rnd.Int;
		var input = R.Wrap(value);

		// Act
		var result = input.Unsafe();

		// Assert
		Assert.Equal(input, result.Value);
	}

	[Fact]
	public void With_Failure__Returns_Unsafe_Wrapping_Failure()
	{
		// Arrange
		var input = FailGen.Create<int>();

		// Act
		var result = input.Unsafe();

		// Assert
		Assert.Equal(input, result.Value);
	}

	[Fact]
	public async Task Task_Returns_Unsafe_Wrapping_Result()
	{
		// Arrange
		var value = Rnd.Int;
		var input = R.Wrap(value);

		// Act
		var result = await input.AsTask().Unsafe();

		// Assert
		Assert.Equal(input, result.Value);
	}

	[Fact]
	public async Task Task_With_Failure__Returns_Unsafe_Wrapping_Failure()
	{
		// Arrange
		var input = FailGen.Create<int>();

		// Act
		var result = await input.AsTask().Unsafe();

		// Assert
		Assert.Equal(input, result.Value);
	}
}
