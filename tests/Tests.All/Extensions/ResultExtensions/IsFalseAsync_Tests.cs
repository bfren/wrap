// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class IsFalseAsync_Tests
{
	public class With_Failure
	{
		[Fact]
		public async Task Returns_False()
		{
			// Arrange
			var input = FailGen.Create<bool>().AsTask();

			// Act
			var result = await input.IsFalseAsync();

			// Assert
			Assert.False(result);
		}
	}

	public class With_Ok
	{
		public class Value_Is_False
		{
			[Fact]
			public async Task Returns_True()
			{
				// Arrange
				var input = R.Wrap(false).AsTask();

				// Act
				var result = await input.IsFalseAsync();

				// Assert
				Assert.True(result);
			}
		}

		public class Value_Is_True
		{
			[Fact]
			public async Task Returns_False()
			{
				// Arrange
				var input = R.Wrap(true).AsTask();

				// Act
				var result = await input.IsFalseAsync();

				// Assert
				Assert.False(result);
			}
		}
	}
}
