// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class IsFalse_Tests
{
	public class With_None
	{
		[Fact]
		public void Returns_False()
		{
			// Arrange
			var input = NoneGen.Create<bool>();

			// Act
			var result = input.IsFalse();

			// Assert
			Assert.False(result);
		}
	}

	public class With_Some
	{
		[Fact]
		public void Value_Is_False__Returns_True()
		{
			// Arrange
			var input = M.Wrap(false);

			// Act
			var result = input.IsFalse();

			// Assert
			Assert.True(result);
		}

		[Fact]
		public void Value_Is_True__Returns_False()
		{
			// Arrange
			var input = M.Wrap(true);

			// Act
			var result = input.IsFalse();

			// Assert
			Assert.False(result);
		}
	}
}
