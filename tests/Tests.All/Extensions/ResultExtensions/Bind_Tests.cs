// Wrap: Unit Tests
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
			var input = FailGen.Create<string>(R.Fail(value));
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

		[Fact]
		public void Failure_Passed_Through_Chain__Is_Unchanged()
		{
			// Arrange
			var failure = FailGen.Create();

			// Act
			var result = R.Fail<string>(failure)
				.Bind(x => R.Wrap(x.Length))
				.Map(x => x + 1)
				.Bind(x => R.Wrap(x.ToString()));

			// Assert
			Assert.Equal(failure, result.AssertFailure());
		}

		[Fact]
		public void Default_Result__Passes_Uninitialised_Failure()
		{
			// Arrange
			var input = default(Result<string>);

			// Act
			var r0 = input.Bind(x => R.Wrap(x.Length));
			var r1 = input.Map(x => x.Length);

			// Assert
			r0.AssertFailure("Uninitialised Failure");
			r1.AssertFailure("Uninitialised Failure");
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
			f.Invoke(Arg.Any<string>()).Returns(FailGen.Create<int>(R.Fail(failValue)));

			// Act
			var result = input.Bind(f);

			// Assert
			result.AssertFailure(failValue);
		}
	}
}
