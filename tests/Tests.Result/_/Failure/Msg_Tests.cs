// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Failure_Tests;

public class Msg_Tests
{
	public class With_Null
	{
		[Fact]
		public void Does_Not_Set_Message_Or_Args()
		{
			// Arrange
			var failure = FailGen.Create();

			// Act
			var result = failure.Msg(null);

			// Assert
			Assert.Equal(failure.Message, result.Message);
			Assert.Equal(failure.Args, result.Args);
		}
	}

	public class With_Message
	{
		[Fact]
		public void Sets_Message()
		{
			// Arrange
			var failure = FailGen.Create();
			var message = Rnd.Str;

			// Act
			var result = failure.Msg(message);

			// Assert
			Assert.Equal(message, result.Message);
			Assert.Empty(result.Args);
		}
	}

	public class With_Message_And_Args
	{
		[Fact]
		public void Sets_Message_And_Args()
		{
			// Arrange
			var failure = FailGen.Create();
			var message = Rnd.Str;
			var args = new object?[] { Rnd.Int, Rnd.Str, Rnd.Date };

			// Act
			var result = failure.Msg(message, args);

			// Assert
			Assert.Equal(message, result.Message);
			Assert.Equal(args, result.Args);
		}
	}
}
