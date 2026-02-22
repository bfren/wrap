// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class IsFalseAsync_Tests
{
	public class With_None
	{
		[Fact]
		public async Task Returns_False()
		{
			// Arrange
			var input = NoneGen.Create<bool>();

			// Act
			var result = await input.AsTask().IsFalseAsync();

			// Assert
			Assert.False(result);
		}
	}

	public class With_Some
	{
		[Fact]
		public async Task Value_Is_False__Returns_True()
		{
			// Arrange
			var input = M.Wrap(false);

			// Act
			var result = await input.AsTask().IsFalseAsync();

			// Assert
			Assert.True(result);
		}

		[Fact]
		public async Task Value_Is_True__Returns_False()
		{
			// Arrange
			var input = M.Wrap(true);

			// Act
			var result = await input.AsTask().IsFalseAsync();

			// Assert
			Assert.False(result);
		}
	}
}
