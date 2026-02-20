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
			var failure = FailGen.Create<int>();

			// Act
			var result = await Task.FromResult(failure.Unsafe()).DiscardAsync();

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
			var wrapped = R.Wrap(value);

			// Act
			var result = await Task.FromResult(wrapped.Unsafe()).DiscardAsync();

			// Assert
			result.AssertSome(value);
		}
	}
}
