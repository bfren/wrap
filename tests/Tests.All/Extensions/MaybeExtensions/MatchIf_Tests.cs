// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class MatchIf_Tests
{
	private static (Func<string> fNone, Func<int, bool> fTest, Func<int, string> fFalse, Func<int, string> fTrue)
		Setup(string noneValue, bool testReturn, string falseValue, string trueValue)
	{
		var fNone = Substitute.For<Func<string>>();
		fNone.Invoke().Returns(noneValue);

		var fTest = Substitute.For<Func<int, bool>>();
		fTest.Invoke(Arg.Any<int>()).Returns(testReturn);

		var fFalse = Substitute.For<Func<int, string>>();
		fFalse.Invoke(Arg.Any<int>()).Returns(falseValue);

		var fTrue = Substitute.For<Func<int, string>>();
		fTrue.Invoke(Arg.Any<int>()).Returns(trueValue);

		return (fNone, fTest, fFalse, fTrue);
	}

	public class With_None
	{
		[Fact]
		public void Calls_FNone_And_Does_Not_Call_Others()
		{
			// Arrange
			Maybe<int> input = M.None;
			var (fNone, fTest, fFalse, fTrue) = Setup(Rnd.Str, false, Rnd.Str, Rnd.Str);

			// Act
			_ = input.MatchIf(fNone, fTest, fFalse, fTrue);

			// Assert
			fNone.Received(1).Invoke();
			fTest.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			fFalse.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			fTrue.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
		}

		[Fact]
		public void Returns_FNone_Value()
		{
			// Arrange
			Maybe<int> input = M.None;
			var value = Rnd.Str;
			var (fNone, fTest, fFalse, fTrue) = Setup(value, false, Rnd.Str, Rnd.Str);

			// Act
			var result = input.MatchIf(fNone, fTest, fFalse, fTrue);

			// Assert
			Assert.Equal(value, result);
		}
	}

	public class With_Some
	{
		public class Predicate_Returns_False
		{
			[Fact]
			public void Calls_FTest_And_FFalse_Not_FTrue()
			{
				// Arrange
				var value = Rnd.Int;
				var input = M.Wrap(value);
				var (fNone, fTest, fFalse, fTrue) = Setup(Rnd.Str, false, Rnd.Str, Rnd.Str);

				// Act
				_ = input.MatchIf(fNone, fTest, fFalse, fTrue);

				// Assert
				fTest.Received(1).Invoke(value);
				fFalse.Received(1).Invoke(value);
				fTrue.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
				fNone.DidNotReceive().Invoke();
			}

			[Fact]
			public void Returns_FFalse_Value()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(Rnd.Int);
				var (fNone, fTest, fFalse, fTrue) = Setup(Rnd.Str, false, value, Rnd.Str);

				// Act
				var result = input.MatchIf(fNone, fTest, fFalse, fTrue);

				// Assert
				Assert.Equal(value, result);
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public void Calls_FTest_And_FTrue_Not_FFalse()
			{
				// Arrange
				var value = Rnd.Int;
				var input = M.Wrap(value);
				var (fNone, fTest, fFalse, fTrue) = Setup(Rnd.Str, true, Rnd.Str, Rnd.Str);

				// Act
				_ = input.MatchIf(fNone, fTest, fFalse, fTrue);

				// Assert
				fTest.Received(1).Invoke(value);
				fTrue.Received(1).Invoke(value);
				fFalse.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
				fNone.DidNotReceive().Invoke();
			}

			[Fact]
			public void Returns_FTrue_Value()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(Rnd.Int);
				var (fNone, fTest, fFalse, fTrue) = Setup(Rnd.Str, true, Rnd.Str, value);

				// Act
				var result = input.MatchIf(fNone, fTest, fFalse, fTrue);

				// Assert
				Assert.Equal(value, result);
			}
		}
	}
}
