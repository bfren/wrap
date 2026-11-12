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
			var left = Assert.IsType<Left<string>>(result.Value);
			Assert.Equal(value, left.Value);
		}

		[Fact]
		public void Null_Value__Stays_Left()
		{
			// Arrange

			// Act
			Either<string?, int> result = new Left<string?>(null);

			// Assert
			var left = Assert.IsType<Left<string?>>(result.Value);
			Assert.Null(left.Value);
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
			var right = Assert.IsType<Right<int>>(result.Value);
			Assert.Equal(value, right.Value);
		}

		[Fact]
		public void Null_Value__Stays_Right()
		{
			// Arrange

			// Act
			Either<int, string?> result = new Right<string?>(null);

			// Assert
			var right = Assert.IsType<Right<string?>>(result.Value);
			Assert.Null(right.Value);
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
