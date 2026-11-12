// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Failure_Tests;

public class Constructor_Tests
{
	public class With_Message_And_Args
	{
		[Fact]
		public void Sets_Message_Args_And_Warning_Level()
		{
			// Arrange
			var message = Rnd.Str;
			var args = new object?[] { Rnd.Int, Rnd.Str, Rnd.Date };

			// Act
			var result = new Failure(message, args);

			// Assert
			Assert.Equal(message, result.Message);
			Assert.Equal(args, result.Args);
			Assert.Equal(Logging.LogLevel.Warning, result.Level);
		}
	}

	public class With_Exception
	{
		[Fact]
		public void Sets_Exception_Message_And_Error_Level()
		{
			// Arrange
			var ex = new Exception(Rnd.Str);

			// Act
			var result = new Failure(ex);

			// Assert
			Assert.Equal(ex, result.Exception);
			Assert.Equal(ex.Message, result.Message);
			Assert.Equal(Logging.LogLevel.Error, result.Level);
		}
	}
}
