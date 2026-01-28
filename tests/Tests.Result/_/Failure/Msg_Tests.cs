// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Failure_Tests;

public class Msg_Tests
{
	public class With_Message
	{
		[Fact]
		public void Sets_Message_In_FailureValue()
		{
			// Arrange
			var failure = FailGen.Create();
			var message = Rnd.Str;

			// Act
			var result = failure.Msg(message);

			// Assert
			Assert.Equal(message, result.Value.Message);
			Assert.Empty(result.Value.Args);
		}
	}

	public class With_Message_And_Args
	{
		[Fact]
		public void Sets_Message_And_Args_In_FailureValue()
		{
			// Arrange
			var failure = FailGen.Create();
			var message = Rnd.Str;
			var args = new object?[] { Rnd.Int, Rnd.Str, Rnd.Date };

			// Act
			var result = failure.Msg(message, args);

			// Assert
			Assert.Equal(message, result.Value.Message);
			Assert.Equal(args, result.Value.Args);
		}
	}
}
