// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class BindIf_Tests
{
	private static (Func<string, bool> fTest, Func<string, Maybe<int>> bind) Setup(bool predicateReturn)
	{
		var fTest = Substitute.For<Func<string, bool>>();
		fTest.Invoke(Arg.Any<string>()).Returns(predicateReturn);

		var bind = Substitute.For<Func<string, Maybe<int>>>();

		return (fTest, bind);
	}

	public class With_None
	{
		public class Predicate_Returns_False
		{
			[Fact]
			public void Returns_None()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var (fTest, bind) = Setup(false);

				// Act
				var result = input.BindIf(fTest, bind);

				// Assert
				result.AssertNone();
			}

			[Fact]
			public void Function_Is_Not_Invoked()
			{
				// Arrange
				var input = NoneGen.Create<string>();
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
			public void Returns_None()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var (fTest, bind) = Setup(true);

				// Act
				var result = input.BindIf(fTest, bind);

				// Assert
				result.AssertNone();
			}

			[Fact]
			public void Function_Is_Not_Invoked()
			{
				// Arrange
				var input = NoneGen.Create<string>();
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
				var input = M.Wrap(Rnd.Str);
				var (fTest, bind) = Setup(false);

				// Act
				var result = input.BindIf(fTest, bind);

				// Assert
				result.AssertNone();
			}

			[Fact]
			public void Function_Is_Not_Invoked()
			{
				// Arrange
				var input = M.Wrap(Rnd.Str);
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
				var input = M.Wrap(Rnd.Str);
				var (fTest, bind) = Setup(true);
				bind.Invoke(Arg.Any<string>()).Returns(value);

				// Act
				var result = input.BindIf(fTest, bind);

				// Assert
				result.AssertSome(value);
			}
		}
	}
}
