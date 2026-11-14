// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EitherExtensions_Tests;

public class Unsafe_Tests
{
	public class With_Left
	{
		[Fact]
		public async Task Returns_Left_Value()
		{
			// Arrange
			Either<string, int> input = new Left<string>(Rnd.Str);

			// Act
			var r0 = input.Unsafe();
			var r1 = await input.AsTask().Unsafe();

			// Assert
			Assert.Equal(input, r0.Value);
			Assert.Equal(input, r1.Value);
		}
	}

	public class With_Right
	{
		[Fact]
		public async Task Returns_Value()
		{
			// Arrange
			Either<string, int> input = new Right<int>(Rnd.Int);

			// Act
			var r0 = input.Unsafe();
			var r1 = await input.AsTask().Unsafe();

			// Assert
			Assert.Equal(input, r0.Value);
			Assert.Equal(input, r1.Value);
		}
	}
}
