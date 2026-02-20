// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.UnsafeExtensions_Tests;

public class Discard_Tests
{
	public class With_Left
	{
		[Fact]
		public void Returns_None()
		{
			// Arrange
			var failure = FailGen.Create<int>();

			// Act
			var result = failure.Unsafe().Discard();

			// Assert
			result.AssertNone();
		}
	}

	public class With_Right
	{
		[Fact]
		public void Returns_Some_With_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var wrapped = R.Wrap(value);

			// Act
			var result = wrapped.Unsafe().Discard();

			// Assert
			result.AssertSome(value);
		}
	}
}
