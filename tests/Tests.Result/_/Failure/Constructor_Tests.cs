// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Failure_Tests;

public class Constructor_Tests
{
	public class With_FailureValue
	{
		[Fact]
		public void Sets_Value_Property()
		{
			// Arrange
			var value = FailGen.Value;

			// Act
			var result = new Failure(value);

			// Assert
			Assert.Equal(value, result.Value);
		}
	}

	public class With_Message_And_Args
	{
		[Fact]
		public void Sets_Value_Property()
		{
			// Arrange
			var message = Rnd.Str;
			var args = new object?[] { Rnd.Int, Rnd.Str, Rnd.Date };

			// Act
			var result = new Failure(message, args);

			// Assert
			Assert.Equal(message, result.Value.Message);
			Assert.Equal(args, result.Value.Args);
		}
	}

	public class With_Exception
	{
		[Fact]
		public void Sets_Value_Property()
		{
			// Arrange
			var ex = new Exception(Rnd.Str);

			// Act
			var result = new Failure(ex);

			// Assert
			Assert.Equal(ex, result.Value.Exception);
		}
	}
}
