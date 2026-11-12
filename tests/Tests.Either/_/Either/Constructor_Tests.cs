// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Either_Tests;

public class Constructor_Tests
{
	public class With_Left
	{
		[Fact]
		public void Converts_To_Left()
		{
			// Arrange
			var value = Rnd.Str;

			// Act
			Either<string, int> result = new Left<string>(value);

			// Assert
			result.AssertLeft(value);
		}

		[Fact]
		public void Null_Value__Stays_Left()
		{
			// Arrange

			// Act
			Either<string?, int> result = new Left<string?>(null);

			// Assert
			Assert.Null(result.AssertLeft());
		}
	}

	public class With_Right
	{
		[Fact]
		public void Converts_To_Right()
		{
			// Arrange
			var value = Rnd.Int;

			// Act
			Either<string, int> result = new Right<int>(value);

			// Assert
			result.AssertRight(value);
		}

		[Fact]
		public void Null_Value__Stays_Right()
		{
			// Arrange

			// Act
			Either<int, string?> result = new Right<string?>(null);

			// Assert
			Assert.Null(result.AssertRight());
		}
	}

	public class With_Same_Types
	{
		[Fact]
		public void Left_And_Right_Are_Different_Cases()
		{
			// Arrange
			var value = Rnd.Int;

			// Act
			Either<int, int> left = new Left<int>(value);
			Either<int, int> right = new Right<int>(value);

			// Assert
			_ = Assert.IsType<Left<int>>(left.Value);
			_ = Assert.IsType<Right<int>>(right.Value);
		}
	}
}
