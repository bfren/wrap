// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class If_Tests
{
	private static (Func<string, bool> fTest, Func<string, Maybe<int>> fTrue, Func<string, Maybe<int>> fFalse) Setup(bool predicateReturn, int trueValue, int falseValue)
	{
		var fTest = Substitute.For<Func<string, bool>>();
		fTest.Invoke(Arg.Any<string>()).Returns(predicateReturn);

		var fTrue = Substitute.For<Func<string, Maybe<int>>>();
		fTrue.Invoke(Arg.Any<string>()).Returns(trueValue);

		var fFalse = Substitute.For<Func<string, Maybe<int>>>();
		fFalse.Invoke(Arg.Any<string>()).Returns(falseValue);

		return (fTest, fTrue, fFalse);
	}

	public class With_FTrue_And_FFalse
	{
		public class With_None
		{
			public class Predicate_Returns_False
			{
				[Fact]
				public void Returns_None()
				{
					// Arrange
					var input = NoneGen.Create<string>();
					var (fTest, fTrue, fFalse) = Setup(false, Rnd.Int, Rnd.Int);

					// Act
					var result = input.If(fTest, fTrue, fFalse);

					// Assert
					result.AssertNone();
				}

				[Fact]
				public void Functions_Are_Not_Invoked()
				{
					// Arrange
					var input = NoneGen.Create<string>();
					var (fTest, fTrue, fFalse) = Setup(false, Rnd.Int, Rnd.Int);

					// Act
					_ = input.If(fTest, fTrue, fFalse);

					// Assert
					fTest.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
					fTrue.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
					fFalse.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
				}
			}

			public class Predicate_Returns_True
			{
				[Fact]
				public void Returns_None()
				{
					// Arrange
					var input = NoneGen.Create<string>();
					var (fTest, fTrue, fFalse) = Setup(true, Rnd.Int, Rnd.Int);

					// Act
					var result = input.If(fTest, fTrue, fFalse);

					// Assert
					result.AssertNone();
				}

				[Fact]
				public void Functions_Are_Not_Invoked()
				{
					// Arrange
					var input = NoneGen.Create<string>();
					var (fTest, fTrue, fFalse) = Setup(true, Rnd.Int, Rnd.Int);

					// Act
					_ = input.If(fTest, fTrue, fFalse);

					// Assert
					fTest.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
					fTrue.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
					fFalse.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
				}
			}
		}

		public class With_Some
		{
			public class Predicate_Returns_False
			{
				[Fact]
				public void Returns_Result_From_FFalse()
				{
					// Arrange
					var value = Rnd.Str;
					var input = M.Wrap(value);
					var falseValue = Rnd.Int;
					var (fTest, fTrue, fFalse) = Setup(false, Rnd.Int, falseValue);

					// Act
					var result = input.If(fTest, fTrue, fFalse);

					// Assert
					result.AssertSome(falseValue);
				}

				[Fact]
				public void Calls_FTest_And_FFalse()
				{
					// Arrange
					var value = Rnd.Str;
					var input = M.Wrap(value);
					var (fTest, fTrue, fFalse) = Setup(false, Rnd.Int, Rnd.Int);

					// Act
					_ = input.If(fTest, fTrue, fFalse);

					// Assert
					fTest.Received().Invoke(value);
					fFalse.Received().Invoke(value);
					fTrue.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
				}
			}

			public class Predicate_Returns_True
			{
				[Fact]
				public void Returns_Result_From_FTrue()
				{
					// Arrange
					var value = Rnd.Str;
					var input = M.Wrap(value);
					var trueValue = Rnd.Int;
					var (fTest, fTrue, fFalse) = Setup(true, trueValue, Rnd.Int);

					// Act
					var result = input.If(fTest, fTrue, fFalse);

					// Assert
					result.AssertSome(trueValue);
				}

				[Fact]
				public void Calls_FTest_And_FTrue()
				{
					// Arrange
					var value = Rnd.Str;
					var input = M.Wrap(value);
					var (fTest, fTrue, fFalse) = Setup(true, Rnd.Int, Rnd.Int);

					// Act
					_ = input.If(fTest, fTrue, fFalse);

					// Assert
					fTest.Received().Invoke(value);
					fTrue.Received().Invoke(value);
					fFalse.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
				}
			}
		}
	}

	public class With_FThen_Only
	{
		private static (Func<string, bool> fTest, Func<string, Maybe<string>> fThen) SetupSameType(bool predicateReturn, string thenValue)
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
					var (fTest, fThen) = SetupSameType(false, Rnd.Str);

					// Act
					var result = input.If(fTest, fThen);

					// Assert
					result.AssertNone();
				}

				[Fact]
				public void Functions_Are_Not_Invoked()
				{
					// Arrange
					var input = NoneGen.Create<string>();
					var (fTest, fThen) = SetupSameType(false, Rnd.Str);

					// Act
					_ = input.If(fTest, fThen);

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
					var (fTest, fThen) = SetupSameType(true, Rnd.Str);

					// Act
					var result = input.If(fTest, fThen);

					// Assert
					result.AssertNone();
				}

				[Fact]
				public void Functions_Are_Not_Invoked()
				{
					// Arrange
					var input = NoneGen.Create<string>();
					var (fTest, fThen) = SetupSameType(true, Rnd.Str);

					// Act
					_ = input.If(fTest, fThen);

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
				public void Returns_Original_Value()
				{
					// Arrange
					var value = Rnd.Str;
					var input = M.Wrap(value);
					var (fTest, fThen) = SetupSameType(false, Rnd.Str);

					// Act
					var result = input.If(fTest, fThen);

					// Assert
					result.AssertSome(value);
				}

				[Fact]
				public void Calls_FTest_Only()
				{
					// Arrange
					var value = Rnd.Str;
					var input = M.Wrap(value);
					var (fTest, fThen) = SetupSameType(false, Rnd.Str);

					// Act
					_ = input.If(fTest, fThen);

					// Assert
					fTest.Received().Invoke(value);
					fThen.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
				}
			}

			public class Predicate_Returns_True
			{
				[Fact]
				public void Returns_Result_From_FThen()
				{
					// Arrange
					var value = Rnd.Str;
					var input = M.Wrap(value);
					var thenValue = Rnd.Str;
					var (fTest, fThen) = SetupSameType(true, thenValue);

					// Act
					var result = input.If(fTest, fThen);

					// Assert
					result.AssertSome(thenValue);
				}

				[Fact]
				public void Calls_FTest_And_FThen()
				{
					// Arrange
					var value = Rnd.Str;
					var input = M.Wrap(value);
					var (fTest, fThen) = SetupSameType(true, Rnd.Str);

					// Act
					_ = input.If(fTest, fThen);

					// Assert
					fTest.Received().Invoke(value);
					fThen.Received().Invoke(value);
				}
			}
		}
	}
}
