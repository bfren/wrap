// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Result_Tests;

public class IsOk_Tests
{
	public class When_Failure
	{
		[Fact]
		public void Returns_False()
		{
			// Arrange
			var value = FailGen.Create<int>();

			// Act
			var result = value.IsOk;

			// Assert
			Assert.False(result);
		}
	}

	public class When_Ok
	{
		[Fact]
		public void Returns_True()
		{
			// Arrange
			var value = R.Wrap(Rnd.Int);

			// Act
			var result = value.IsOk;

			// Assert
			Assert.True(result);
		}
	}
}
