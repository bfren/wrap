// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class MatchIf_Tests
{
	private static (Func<int, bool> fTest, Func<int, string> fFalse, Func<int, string> fTrue) SetupOk(bool predicateReturn)
	{
		var fTest = Substitute.For<Func<int, bool>>();
		fTest.Invoke(Arg.Any<int>()).Returns(predicateReturn);
		var fFalse = Substitute.For<Func<int, string>>();
		var fTrue = Substitute.For<Func<int, string>>();
		return (fTest, fFalse, fTrue);
	}

	public class With_Failure
	{
		[Fact]
		public void Returns_fFail_Result()
		{
			// Arrange
			var failReturn = Rnd.Str;
			var input = FailGen.Create<int>();
			var (fTest, fFalse, fTrue) = SetupOk(true);

			// Act
			var result = input.MatchIf(_ => failReturn, fTest, fFalse, fTrue);

			// Assert
			Assert.Equal(failReturn, result);
		}

		[Fact]
		public void Neither_fFalse_Nor_fTrue_Is_Invoked()
		{
			// Arrange
			var input = FailGen.Create<int>();
			var (fTest, fFalse, fTrue) = SetupOk(true);

			// Act
			_ = input.MatchIf(_ => Rnd.Str, fTest, fFalse, fTrue);

			// Assert
			fFalse.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			fTrue.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
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
				var falseReturn = Rnd.Str;
				var input = R.Wrap(Rnd.Int);
				var (fTest, fFalse, fTrue) = SetupOk(false);
				fFalse.Invoke(Arg.Any<int>()).Returns(falseReturn);

				// Act
				var result = input.MatchIf(_ => Rnd.Str, fTest, fFalse, fTrue);

				// Assert
				Assert.Equal(falseReturn, result);
			}

			[Fact]
			public void fTest_Is_Invoked()
			{
				// Arrange
				var value = Rnd.Int;
				var input = R.Wrap(value);
				var (fTest, fFalse, fTrue) = SetupOk(false);
				fFalse.Invoke(Arg.Any<int>()).Returns(Rnd.Str);

				// Act
				_ = input.MatchIf(_ => Rnd.Str, fTest, fFalse, fTrue);

				// Assert
				fTest.Received(1).Invoke(value);
			}

			[Fact]
			public void fTrue_Is_Not_Invoked()
			{
				// Arrange
				var input = R.Wrap(Rnd.Int);
				var (fTest, fFalse, fTrue) = SetupOk(false);
				fFalse.Invoke(Arg.Any<int>()).Returns(Rnd.Str);

				// Act
				_ = input.MatchIf(_ => Rnd.Str, fTest, fFalse, fTrue);

				// Assert
				fTrue.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public void Returns_fTrue_Result()
			{
				// Arrange
				var trueReturn = Rnd.Str;
				var input = R.Wrap(Rnd.Int);
				var (fTest, fFalse, fTrue) = SetupOk(true);
				fTrue.Invoke(Arg.Any<int>()).Returns(trueReturn);

				// Act
				var result = input.MatchIf(_ => Rnd.Str, fTest, fFalse, fTrue);

				// Assert
				Assert.Equal(trueReturn, result);
			}

			[Fact]
			public void fTest_Is_Invoked()
			{
				// Arrange
				var value = Rnd.Int;
				var input = R.Wrap(value);
				var (fTest, fFalse, fTrue) = SetupOk(true);
				fTrue.Invoke(Arg.Any<int>()).Returns(Rnd.Str);

				// Act
				_ = input.MatchIf(_ => Rnd.Str, fTest, fFalse, fTrue);

				// Assert
				fTest.Received(1).Invoke(value);
			}

			[Fact]
			public void fFalse_Is_Not_Invoked()
			{
				// Arrange
				var input = R.Wrap(Rnd.Int);
				var (fTest, fFalse, fTrue) = SetupOk(true);
				fTrue.Invoke(Arg.Any<int>()).Returns(Rnd.Str);

				// Act
				_ = input.MatchIf(_ => Rnd.Str, fTest, fFalse, fTrue);

				// Assert
				fFalse.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			}
		}
	}
}
