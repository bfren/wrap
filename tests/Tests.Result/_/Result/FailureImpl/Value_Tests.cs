// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Result_Tests.FailureImpl_Tests;

public class Value_Tests
{
	public class When_LogException_Is_Not_Null
	{
		public class With_Exception
		{
			[Fact]
			public void Calls_Log_With_Exception()
			{
				// Arrange
				var logger = Substitute.For<F.ExceptionLogger>();
				var ex = new Exception(Rnd.Str);
				var value = FailGen.Create(ex).Value;

				// Act
				_ = new Result<int>.FailureImpl(logger, null, value);

				// Assert
				logger.Received(1).Invoke(ex);
			}
		}

		[Fact]
		public void Sets_Value()
		{
			// Arrange
			var ex = new Exception(Rnd.Str);
			var value = FailGen.Create(ex).Value;

			// Act
			var result = new Result<int>.FailureImpl(Substitute.For<F.ExceptionLogger>(), null, value);

			// Assert
			Assert.Equal(value, result.Value);
		}
	}

	public class When_FailureException_Is_Not_Null
	{
		[Fact]
		public void Calls_Log_With_Message_And_Args()
		{
			// Arrange
			var logger = Substitute.For<F.FailureLogger>();
			var value = FailGen.Value;

			// Act
			_ = new Result<string>.FailureImpl(null, logger, value);

			// Assert
			logger.Received(1).Invoke(value.Message, value.Args);
		}

		[Fact]
		public void Sets_Value()
		{
			// Arrange
			var value = FailGen.Value;

			// Act
			var result = new Result<string>.FailureImpl(null, Substitute.For<F.FailureLogger>(), value);

			// Assert
			Assert.Equal(value, result.Value);
		}
	}
}
