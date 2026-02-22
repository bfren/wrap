// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class IfNot_Tests
{
	private static (Func<string, bool> fTest, Func<string, Maybe<string>> fThen) Setup(bool predicateReturn, string thenValue)
	{
		var fTest = Substitute.For<Func<string, bool>>();
		fTest.Invoke(Arg.Any<string>()).Returns(predicateReturn);

		var fThen = Substitute.For<Func<string, Maybe<string>>>();
		fThen.Invoke(Arg.Any<string>()).Returns(thenValue);

		return (fTest, fThen);
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
				var (fTest, fThen) = Setup(false, Rnd.Str);

				// Act
				var result = input.IfNot(fTest, fThen);

				// Assert
				result.AssertNone();
			}

			[Fact]
			public void Functions_Are_Not_Invoked()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var (fTest, fThen) = Setup(false, Rnd.Str);

				// Act
				_ = input.IfNot(fTest, fThen);

				// Assert
				fTest.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
				fThen.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public void Returns_None()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var (fTest, fThen) = Setup(true, Rnd.Str);

				// Act
				var result = input.IfNot(fTest, fThen);

				// Assert
				result.AssertNone();
			}

			[Fact]
			public void Functions_Are_Not_Invoked()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var (fTest, fThen) = Setup(true, Rnd.Str);

				// Act
				_ = input.IfNot(fTest, fThen);

				// Assert
				fTest.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
				fThen.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			}
		}
	}

	public class With_Some
	{
		public class Predicate_Returns_False
		{
			[Fact]
			public void Returns_Result_From_FThen()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var thenValue = Rnd.Str;
				var (fTest, fThen) = Setup(false, thenValue);

				// Act
				var result = input.IfNot(fTest, fThen);

				// Assert
				result.AssertSome(thenValue);
			}

			[Fact]
			public void Calls_FTest_And_FThen()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var (fTest, fThen) = Setup(false, Rnd.Str);

				// Act
				_ = input.IfNot(fTest, fThen);

				// Assert
				fTest.Received().Invoke(value);
				fThen.Received().Invoke(value);
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public void Returns_Original_Value()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var (fTest, fThen) = Setup(true, Rnd.Str);

				// Act
				var result = input.IfNot(fTest, fThen);

				// Assert
				result.AssertSome(value);
			}

			[Fact]
			public void Calls_FTest_Only()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var (fTest, fThen) = Setup(true, Rnd.Str);

				// Act
				_ = input.IfNot(fTest, fThen);

				// Assert
				fTest.Received().Invoke(value);
				fThen.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			}
		}
	}
}
