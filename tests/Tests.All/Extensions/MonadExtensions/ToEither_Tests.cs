// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MonadExtensions_Tests;

public class ToEither_Tests
{
	public class As_Left
	{
		[Fact]
		public void Returns_Left_With_Value()
		{
			// Arrange
			var value = Rnd.Str;
			var monad = Monad<string>.Wrap(value);

			// Act
			var result = monad.ToEither<string, int>();

			// Assert
			var left = Assert.IsType<Left<string, int>>(result);
			Assert.Equal(value, left.Value);
		}
	}

	public class As_Right
	{
		[Fact]
		public void Returns_Right_With_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var monad = Monad<int>.Wrap(value);

			// Act
			var result = monad.ToEither<string, int>();

			// Assert
			var right = Assert.IsType<Right<string, int>>(result);
			Assert.Equal(value, right.Value);
		}
	}
}
