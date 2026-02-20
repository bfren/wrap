// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.UnsafeExtensions_Tests;

public class Unwrap_Tests
{
	public class With_Left
	{
		[Fact]
		public void Returns_Default()
		{
			// Arrange
			var failure = FailGen.Create<int>();

			// Act
			var result = failure.Unsafe().Unwrap();

			// Assert
			Assert.Equal(default, result);
		}
	}

	public class With_Right
	{
		[Fact]
		public void Returns_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var wrapped = R.Wrap(value);

			// Act
			var result = wrapped.Unsafe().Unwrap();

			// Assert
			Assert.Equal(value, result);
		}
	}
}
