// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Exceptions;

namespace Wrap.Extensions.UnsafeExtensions_Tests;

public class UnwrapAsync_Tests
{
	public class With_Left
	{
		[Fact]
		public async Task Throws_UnsafeUnwrapException()
		{
			// Arrange
			var failure = R.Fail<int>(Rnd.Str).AsTask().Unsafe();

			// Act
			var result = await Record.ExceptionAsync(() => failure.UnwrapAsync());

			// Assert
			_ = Assert.IsType<UnsafeUnwrapException>(result);
		}
	}

	public class With_Right
	{
		[Fact]
		public async Task Returns_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var wrapped = R.Wrap(value).AsTask().Unsafe();

			// Act
			var result = await wrapped.UnwrapAsync();

			// Assert
			Assert.Equal(value, result);
		}
	}
}
