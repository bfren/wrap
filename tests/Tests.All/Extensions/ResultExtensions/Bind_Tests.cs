// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class Bind_Tests
{
	public class With_Failure
	{
		[Fact]
		public void Returns_Failure()
		{
			// Arrange
			var value = Rnd.Str;
			var input = FailGen.Create<string>(new(value));
			var f = Substitute.For<Func<string, Result<int>>>();

			// Act
			var result = input.Bind(f);

			// Assert
			result.AssertFailure(value);
		}

		[Fact]
		public void f_Is_Not_Invoked()
		{
			// Arrange
			var input = FailGen.Create<string>();
			var f = Substitute.For<Func<string, Result<int>>>();

			// Act
			_ = input.Bind(f);

			// Assert
			f.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
		}
	}

	public class With_Ok
	{
		[Fact]
		public void Returns_Result_Of_f()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Wrap(Rnd.Str);
			var f = Substitute.For<Func<string, Result<int>>>();
			f.Invoke(Arg.Any<string>()).Returns(value);

			// Act
			var result = input.Bind(f);

			// Assert
			result.AssertOk(value);
		}

		[Fact]
		public void f_Is_Invoked_With_Value()
		{
			// Arrange
			var value = Rnd.Str;
			var input = R.Wrap(value);
			var f = Substitute.For<Func<string, Result<int>>>();
			f.Invoke(Arg.Any<string>()).Returns(Rnd.Int);

			// Act
			_ = input.Bind(f);

			// Assert
			f.Received(1).Invoke(value);
		}

		[Fact]
		public void f_Returns_Failure__Returns_Failure()
		{
			// Arrange
			var failValue = Rnd.Str;
			var input = R.Wrap(Rnd.Str);
			var f = Substitute.For<Func<string, Result<int>>>();
			f.Invoke(Arg.Any<string>()).Returns(FailGen.Create<int>(new(failValue)));

			// Act
			var result = input.Bind(f);

			// Assert
			result.AssertFailure(failValue);
		}
	}
}
