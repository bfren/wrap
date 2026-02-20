// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.UnsafeExtensions_Tests;

public class DiscardAsync_Tests
{
	public class With_Left
	{
		[Fact]
		public async Task Returns_None()
		{
			// Arrange
			var failure = FailGen.Create<int>().AsTask().Unsafe();

			// Act
			var result = await failure.DiscardAsync();

			// Assert
			result.AssertNone();
		}
	}

	public class With_Right
	{
		[Fact]
		public async Task Returns_Some_With_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var wrapped = R.Wrap(value).AsTask().Unsafe();

			// Act
			var result = await wrapped.DiscardAsync();

			// Assert
			result.AssertSome(value);
		}
	}
}
