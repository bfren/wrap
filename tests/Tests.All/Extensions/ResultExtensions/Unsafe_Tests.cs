// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class Unsafe_Tests
{
	public class With_Failure
	{
		[Fact]
		public async Task Returns_Failure_Value()
		{
			// Arrange
			var input = FailGen.Create<int>();

			// Act
			var r0 = input.Unsafe();
			var r1 = await input.AsTask().Unsafe();

			// Assert
			Assert.Equal(input, r0.Value);
			Assert.Equal(input, r1.Value);
		}
	}

	public class With_Ok
	{
		[Fact]
		public async Task Returns_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Wrap(value);

			// Act
			var r0 = input.Unsafe();
			var r1 = await input.AsTask().Unsafe();

			// Assert
			Assert.Equal(input, r0.Value);
			Assert.Equal(input, r1.Value);
		}
	}
}
