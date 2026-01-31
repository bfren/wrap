// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.FailureValue_Tests;

public class Constructor_Tests
{
	public class With_Message_And_Args
	{
		[Fact]
		public void Sets_Properties_Correctly()
		{
			// Arrange
			var message = Rnd.Str;
			var a0 = Rnd.Int;
			var a1 = Rnd.Guid;

			// Act
			var result = new FailureValue(message, a0, a1);

			// Assert
			Assert.Equal(message, result.Message);
			Assert.Null(result.Context);
			Assert.Null(result.Exception);
			Assert.Equal(FailureValue.DefaultFailureLevel, result.Level);
			Assert.Equal([a0, a1], result.Args);
		}
	}

	public class With_Exception
	{
		[Fact]
		public void Sets_Properties_Correctly()
		{
			// Arrange
			var ex = new Exception(Rnd.Str);

			// Act
			var result = new FailureValue(ex);

			// Assert
			Assert.Equal(ex.Message, result.Message);
			Assert.Null(result.Context);
			Assert.Equal(ex, result.Exception);
			Assert.Equal(FailureValue.DefaultExceptionLevel, result.Level);
			Assert.Empty(result.Args);
		}
	}
}
