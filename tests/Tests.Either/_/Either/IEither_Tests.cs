// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Either_Tests;

public class IEither_Tests
{
	public class With_Left
	{
		[Fact]
		public void TryGetLeft_Returns_True_And_Value()
		{
			// Arrange
			var value = Rnd.Str;
			IEither<string, int> either = (Either<string, int>)new Left<string>(value);

			// Act
			var result = either.TryGetLeft(out var left);

			// Assert
			Assert.True(result);
			Assert.Equal(value, left);
		}

		[Fact]
		public void TryGetRight_Returns_False()
		{
			// Arrange
			IEither<string, int> either = (Either<string, int>)new Left<string>(Rnd.Str);

			// Act
			var result = either.TryGetRight(out _);

			// Assert
			Assert.False(result);
		}
	}

	public class With_Right
	{
		[Fact]
		public void TryGetRight_Returns_True_And_Value()
		{
			// Arrange
			var value = Rnd.Int;
			IEither<string, int> either = (Either<string, int>)new Right<int>(value);

			// Act
			var result = either.TryGetRight(out var right);

			// Assert
			Assert.True(result);
			Assert.Equal(value, right);
		}

		[Fact]
		public void TryGetLeft_Returns_False()
		{
			// Arrange
			IEither<string, int> either = (Either<string, int>)new Right<int>(Rnd.Int);

			// Act
			var result = either.TryGetLeft(out _);

			// Assert
			Assert.False(result);
		}
	}
}
