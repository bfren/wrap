// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.UnsafeExtensions_Tests;

public class UnwrapAsync_Tests
{
	public class With_Left
	{
		[Fact]
		public async Task Returns_Default()
		{
			// Arrange
			var failure = FailGen.Create<int>();

			// Act
			var result = await Task.FromResult(failure.Unsafe()).UnwrapAsync();

			// Assert
			Assert.Equal(default, result);
		}
	}

	public class With_Right
	{
		[Fact]
		public async Task Returns_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var wrapped = R.Wrap(value);

			// Act
			var result = await Task.FromResult(wrapped.Unsafe()).UnwrapAsync();

			// Assert
			Assert.Equal(value, result);
		}
	}
}
