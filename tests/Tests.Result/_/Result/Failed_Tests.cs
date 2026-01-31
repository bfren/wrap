// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Result_Tests;

public class Failed_Tests
{
	public class When_Failure
	{
		[Fact]
		public void Returns_True()
		{
			// Arrange
			var value = FailGen.Create<int>();

			// Act
			var result = value.Failed;

			// Assert
			Assert.True(result);
		}
	}

	public class When_Ok
	{
		[Fact]
		public void Returns_False()
		{
			// Arrange
			var value = R.Wrap(Rnd.Int);

			// Act
			var result = value.Failed;

			// Assert
			Assert.False(result);
		}
	}
}
