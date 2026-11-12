// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Either_Tests;

public class ImplicitOperator_Tests
{
	public class From_TLeft
	{
		[Fact]
		public void Returns_Left_With_Value()
		{
			// Arrange
			var value = Rnd.Str;

			// Act
			Either<string, int> result = value;

			// Assert
			result.AssertLeft(value);
		}
	}

	public class From_TRight
	{
		[Fact]
		public void Returns_Right_With_Value()
		{
			// Arrange
			var value = Rnd.Int;

			// Act
			Either<string, int> result = value;

			// Assert
			result.AssertRight(value);
		}
	}
}
