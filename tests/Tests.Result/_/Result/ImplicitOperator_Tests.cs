// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Result_Tests;

public class ImplicitOperator_Tests
{
	public class With_Value
	{
		[Fact]
		public void Returns_Ok_With_Value()
		{
			// Arrange
			var value = Rnd.Int;

			// Act
			Result<int> result = value;

			// Assert
			result.AssertOk(value);
		}
	}

	public class With_Failure
	{
		[Fact]
		public void Returns_Failure()
		{
			// Arrange
			var value = FailGen.Create();

			// Act
			Result<DateTime> result = value;

			// Assert
			result.AssertFailure();
		}
	}

	public class With_Null
	{
		[Fact]
		public void Returns_Failure()
		{
			// Arrange
			static Result<string> act() => (string)null!;

			// Act
			var result = act();

			// Assert
			result.AssertFailure();
		}
	}
}
