// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class BindIf_Tests
{
	private static (Func<string, bool> fTest, Func<string, Result<int>> bind) Setup(bool predicateReturn)
	{
		var fTest = Substitute.For<Func<string, bool>>();
		fTest.Invoke(Arg.Any<string>()).Returns(predicateReturn);

		var bind = Substitute.For<Func<string, Result<int>>>();

		return (fTest, bind);
	}

	public class With_Failure
	{
		public class Predicate_Returns_False
		{
			[Fact]
			public void Returns_Failure()
			{
				// Arrange
				var value = Rnd.Str;
				var input = FailGen.Create<string>(new(value));
				var (fTest, bind) = Setup(false);

				// Act
				var result = input.BindIf(fTest, bind);

				// Assert
				result.AssertFailure(value);
			}

			[Fact]
			public void Function_Is_Not_Invoked()
			{
				// Arrange
				var input = FailGen.Create<string>();
				var (fTest, bind) = Setup(false);

				// Act
				_ = input.BindIf(fTest, bind);

				// Assert
				bind.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public void Returns_Failure()
			{
				// Arrange
				var value = Rnd.Str;
				var input = FailGen.Create<string>(new(value));
				var (fTest, bind) = Setup(true);

				// Act
				var result = input.BindIf(fTest, bind);

				// Assert
				result.AssertFailure(value);
			}

			[Fact]
			public void Function_Is_Not_Invoked()
			{
				// Arrange
				var input = FailGen.Create<string>();
				var (fTest, bind) = Setup(true);

				// Act
				_ = input.BindIf(fTest, bind);

				// Assert
				bind.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			}
		}
	}

	public class With_Some
	{
		public class Predicate_Returns_False
		{
			[Fact]
			public void Returns_None()
			{
				// Arrange
				var input = R.Wrap(Rnd.Str);
				var (fTest, bind) = Setup(false);

				// Act
				var result = input.BindIf(fTest, bind);

				// Assert
				result.AssertFailure(C.TestFalseMessage);
			}

			[Fact]
			public void Function_Is_Not_Invoked()
			{
				// Arrange
				var input = R.Wrap(Rnd.Str);
				var (fTest, bind) = Setup(false);

				// Act
				_ = input.BindIf(fTest, bind);

				// Assert
				bind.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public void Returns_Some_With_Value()
			{
				// Arrange
				var value = Rnd.Int;
				var input = R.Wrap(Rnd.Str);
				var (fTest, bind) = Setup(true);
				bind.Invoke(Arg.Any<string>()).Returns(value);

				// Act
				var result = input.BindIf(fTest, bind);

				// Assert
				result.AssertOk(value);
			}
		}
	}
}
