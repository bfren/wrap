// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Exceptions;

namespace Wrap.Extensions.UnsafeExtensions_Tests;

public class UnwrapSingleAsync_Tests
{
	private static Func<None, int> Setup(int defaultValue)
	{
		var fLeft = Substitute.For<Func<None, int>>();
		fLeft.Invoke(Arg.Any<None>()).Returns(defaultValue);
		return fLeft;
	}

	public class With_Left
	{
		[Fact]
		public async Task Returns_Value_From_FLeft()
		{
			// Arrange
			var input = NoneGen.Create<List<int>>().AsTask().Unsafe();
			var defaultValue = Rnd.Int;
			var fLeft = Setup(defaultValue);

			// Act
			var result = await input.UnwrapSingleAsync(fLeft);

			// Assert
			Assert.Equal(defaultValue, result);
		}
	}

	public class With_Right
	{
		[Fact]
		public async Task Single_Value__Returns_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var input = M.Wrap(new List<int> { value }).AsTask().Unsafe();
			var fLeft = Setup(Rnd.Int);

			// Act
			var result = await input.UnwrapSingleAsync(fLeft);

			// Assert
			Assert.Equal(value, result);
			fLeft.DidNotReceive().Invoke(Arg.Any<None>());
		}

		[Fact]
		public async Task Multiple_Values__Throws_UnsafeUnwrapSingleException()
		{
			// Arrange
			var input = M.Wrap(new List<int> { Rnd.Int, Rnd.Int }).AsTask().Unsafe();
			var fLeft = Setup(Rnd.Int);

			// Act
			var result = await Record.ExceptionAsync(() => input.UnwrapSingleAsync(fLeft));

			// Assert
			_ = Assert.IsType<UnsafeUnwrapSingleException<List<int>, int>>(result);
		}
	}
}
