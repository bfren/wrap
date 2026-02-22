// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class IsFalse_Tests
{
	public class With_Failure
	{
		[Fact]
		public void Returns_False()
		{
			// Arrange
			var input = FailGen.Create<bool>();

			// Act
			var result = input.IsFalse();

			// Assert
			Assert.False(result);
		}
	}

	public class With_Ok
	{
		public class Value_Is_False
		{
			[Fact]
			public void Returns_True()
			{
				// Arrange
				var input = R.Wrap(false);

				// Act
				var result = input.IsFalse();

				// Assert
				Assert.True(result);
			}
		}

		public class Value_Is_True
		{
			[Fact]
			public void Returns_False()
			{
				// Arrange
				var input = R.Wrap(true);

				// Act
				var result = input.IsFalse();

				// Assert
				Assert.False(result);
			}
		}
	}
}
