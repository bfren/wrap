// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class If_Tests
{
	private static (Func<string, bool> fTest, Func<string, Result<int>> fTrue, Func<string, Result<int>> fFalse) Setup(bool predicateReturn)
	{
		var fTest = Substitute.For<Func<string, bool>>();
		fTest.Invoke(Arg.Any<string>()).Returns(predicateReturn);
		var fTrue = Substitute.For<Func<string, Result<int>>>();
		var fFalse = Substitute.For<Func<string, Result<int>>>();
		return (fTest, fTrue, fFalse);
	}

	public class With_Failure
	{
		[Fact]
		public void Returns_Failure()
		{
			// Arrange
			var value = Rnd.Str;
			var input = FailGen.Create<string>(new(value));
			var (fTest, fTrue, fFalse) = Setup(true);

			// Act
			var result = input.If(fTest, fTrue, fFalse);

			// Assert
			result.AssertFailure(value);
		}

		[Fact]
		public void Neither_fTrue_Nor_fFalse_Is_Invoked()
		{
			// Arrange
			var input = FailGen.Create<string>();
			var (fTest, fTrue, fFalse) = Setup(true);

			// Act
			_ = input.If(fTest, fTrue, fFalse);

			// Assert
			fTrue.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			fFalse.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
		}
	}

	public class With_Ok
	{
		public class Predicate_Returns_False
		{
			[Fact]
			public void Returns_fFalse_Result()
			{
				// Arrange
				var returnValue = Rnd.Int;
				var input = R.Wrap(Rnd.Str);
				var (fTest, fTrue, fFalse) = Setup(false);
				fFalse.Invoke(Arg.Any<string>()).Returns(returnValue);

				// Act
				var result = input.If(fTest, fTrue, fFalse);

				// Assert
				result.AssertOk(returnValue);
			}

			[Fact]
			public void fTrue_Is_Not_Invoked()
			{
				// Arrange
				var input = R.Wrap(Rnd.Str);
				var (fTest, fTrue, fFalse) = Setup(false);
				fFalse.Invoke(Arg.Any<string>()).Returns(Rnd.Int);

				// Act
				_ = input.If(fTest, fTrue, fFalse);

				// Assert
				fTrue.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public void Returns_fTrue_Result()
			{
				// Arrange
				var returnValue = Rnd.Int;
				var input = R.Wrap(Rnd.Str);
				var (fTest, fTrue, fFalse) = Setup(true);
				fTrue.Invoke(Arg.Any<string>()).Returns(returnValue);

				// Act
				var result = input.If(fTest, fTrue, fFalse);

				// Assert
				result.AssertOk(returnValue);
			}

			[Fact]
			public void fFalse_Is_Not_Invoked()
			{
				// Arrange
				var input = R.Wrap(Rnd.Str);
				var (fTest, fTrue, fFalse) = Setup(true);
				fTrue.Invoke(Arg.Any<string>()).Returns(Rnd.Int);

				// Act
				_ = input.If(fTest, fTrue, fFalse);

				// Assert
				fFalse.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			}
		}
	}

	// Act-if-true special case: If<T>(fTest, fThen) returns original value if false
	public class Act_If_True
	{
		public class With_Failure
		{
			[Fact]
			public void Returns_Failure()
			{
				// Arrange
				var value = Rnd.Str;
				var input = FailGen.Create<int>(new(value));
				var fTest = Substitute.For<Func<int, bool>>();
				var fThen = Substitute.For<Func<int, Result<int>>>();

				// Act
				var result = input.If(fTest, fThen);

				// Assert
				result.AssertFailure(value);
			}

			[Fact]
			public void Neither_fTest_Nor_fThen_Is_Invoked()
			{
				// Arrange
				var input = FailGen.Create<int>();
				var fTest = Substitute.For<Func<int, bool>>();
				var fThen = Substitute.For<Func<int, Result<int>>>();

				// Act
				_ = input.If(fTest, fThen);

				// Assert
				fTest.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
				fThen.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			}
		}

		public class Predicate_Returns_False
		{
			[Fact]
			public void Returns_Original_Value()
			{
				// Arrange
				var value = Rnd.Int;
				var input = R.Wrap(value);
				var fTest = Substitute.For<Func<int, bool>>();
				fTest.Invoke(Arg.Any<int>()).Returns(false);
				var fThen = Substitute.For<Func<int, Result<int>>>();

				// Act
				var result = input.If(fTest, fThen);

				// Assert
				result.AssertOk(value);
			}

			[Fact]
			public void fThen_Is_Not_Invoked()
			{
				// Arrange
				var input = R.Wrap(Rnd.Int);
				var fTest = Substitute.For<Func<int, bool>>();
				fTest.Invoke(Arg.Any<int>()).Returns(false);
				var fThen = Substitute.For<Func<int, Result<int>>>();

				// Act
				_ = input.If(fTest, fThen);

				// Assert
				fThen.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public void Returns_fThen_Result()
			{
				// Arrange
				var returnValue = Rnd.Int;
				var input = R.Wrap(Rnd.Int);
				var fTest = Substitute.For<Func<int, bool>>();
				fTest.Invoke(Arg.Any<int>()).Returns(true);
				var fThen = Substitute.For<Func<int, Result<int>>>();
				fThen.Invoke(Arg.Any<int>()).Returns(returnValue);

				// Act
				var result = input.If(fTest, fThen);

				// Assert
				result.AssertOk(returnValue);
			}
		}
	}
}
