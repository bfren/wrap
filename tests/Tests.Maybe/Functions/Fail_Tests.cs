// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Functions_Tests;

public class Fail_Tests
{
	public class With_FailureValue
	{
		[Fact]
		public void Returns_Failure_With_Correct_Value()
		{
			// Arrange
			var value = FailGen.Value;

			// Act
			var r0 = R.Fail(value);
			var r1 = R.Fail<int>(value);

			// Assert
			Assert.Equal(value, r0.Value);
			var f1 = r1.AssertFailure();
			Assert.Equal(value, f1);
		}
	}

	public class With_Message_And_Args
	{
		[Fact]
		public void Returns_Failure_With_Correct_Value()
		{
			// Arrange
			var msg = Rnd.Str;
			var args = new object?[] { Rnd.Date, Rnd.Int, Rnd.Guid };

			// Act
			var r0 = R.Fail(msg, args);
			var r1 = R.Fail<int>(msg, args);

			// Assert
			Assert.Equal(msg, r0.Value.Message);
			Assert.Equal(args, r0.Value.Args);
			var f1 = r1.AssertFailure();
			Assert.Equal(msg, f1.Message);
			Assert.Equal(args, f1.Args);
		}
	}

	public class With_Exception
	{
		[Fact]
		public void Returns_Failure_With_Correct_Value()
		{
			// Arrange
			var ex = new TestException();

			// Act
			var r0 = R.Fail(ex);
			var r1 = R.Fail<TestException>();
			var r2 = R.Fail<string>(ex);
			var r3 = R.Fail<string, TestException>();

			// Assert
			Assert.Same(ex, r0.Value.Exception);
			Assert.IsType<TestException>(r1.Value.Exception);
			var f2 = r2.AssertFailure();
			Assert.Equal(ex, f2.Exception);
			var f3 = r3.AssertFailure();
			Assert.IsType<TestException>(f3.Exception);
		}
	}

	public class TestException : Exception;
}
