// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Maybe_Tests;

public class ImplicitOperator_Tests
{
	public class With_Value
	{
		[Fact]
		public void Returns_Some_With_Value()
		{
			// Arrange
			var value = Rnd.Int;

			// Act
			Maybe<int> result = value;

			// Assert
			result.AssertSome(value);
		}
	}

	public class With_None
	{
		[Fact]
		public void Returns_None()
		{
			// Arrange
			var value = M.None;

			// Act
			Maybe<DateTime> result = value;

			// Assert
			result.AssertNone();
		}
	}

	public class With_Null
	{
		[Fact]
		public void Returns_None()
		{
			// Arrange
			static Maybe<string> act() => (string)null!;

			// Act
			var result = act();

			// Assert
			result.AssertNone();
		}
	}
}
