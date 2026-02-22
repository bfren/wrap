// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Maybe_Tests;

public class IsNone_Tests
{
	public class When_Failure
	{
		[Fact]
		public void Returns_True()
		{
			// Arrange
			var value = NoneGen.Create<int>();

			// Act
			var result = value.IsNone;

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
			var value = M.Wrap(Rnd.Int);

			// Act
			var result = value.IsNone;

			// Assert
			Assert.False(result);
		}
	}
}
