// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class IfAsync_Tests
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
				public async Task Returns_None()
				{
					// Arrange
					var input = NoneGen.Create<string>();
					var (fTest, fTrue, fFalse) = Setup(false, Rnd.Int, Rnd.Int);

					// Act
					var r0 = await input.IfAsync(fTest, x => Task.FromResult(fTrue(x)), x => Task.FromResult(fFalse(x)));
					var r1 = await input.AsTask().IfAsync(fTest, fTrue, fFalse);
					var r2 = await input.AsTask().IfAsync(fTest, x => Task.FromResult(fTrue(x)), x => Task.FromResult(fFalse(x)));

					// Assert
					r0.AssertNone();
					r1.AssertNone();
					r2.AssertNone();
				}

				[Fact]
				public async Task Functions_Are_Not_Invoked()
				{
					// Arrange
					var input = NoneGen.Create<string>();
					var (fTest, fTrue, fFalse) = Setup(false, Rnd.Int, Rnd.Int);

					// Act
					_ = await input.IfAsync(fTest, x => Task.FromResult(fTrue(x)), x => Task.FromResult(fFalse(x)));
					_ = await input.AsTask().IfAsync(fTest, fTrue, fFalse);
					_ = await input.AsTask().IfAsync(fTest, x => Task.FromResult(fTrue(x)), x => Task.FromResult(fFalse(x)));

					// Assert
					fTest.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
					fTrue.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
					fFalse.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
				}
			}

			public class Predicate_Returns_True
			{
				[Fact]
				public async Task Returns_None()
				{
					// Arrange
					var input = NoneGen.Create<string>();
					var (fTest, fTrue, fFalse) = Setup(true, Rnd.Int, Rnd.Int);

					// Act
					var r0 = await input.IfAsync(fTest, x => Task.FromResult(fTrue(x)), x => Task.FromResult(fFalse(x)));
					var r1 = await input.AsTask().IfAsync(fTest, fTrue, fFalse);
					var r2 = await input.AsTask().IfAsync(fTest, x => Task.FromResult(fTrue(x)), x => Task.FromResult(fFalse(x)));

					// Assert
					r0.AssertNone();
					r1.AssertNone();
					r2.AssertNone();
				}

				[Fact]
				public async Task Functions_Are_Not_Invoked()
				{
					// Arrange
					var input = NoneGen.Create<string>();
					var (fTest, fTrue, fFalse) = Setup(true, Rnd.Int, Rnd.Int);

					// Act
					_ = await input.IfAsync(fTest, x => Task.FromResult(fTrue(x)), x => Task.FromResult(fFalse(x)));
					_ = await input.AsTask().IfAsync(fTest, fTrue, fFalse);
					_ = await input.AsTask().IfAsync(fTest, x => Task.FromResult(fTrue(x)), x => Task.FromResult(fFalse(x)));

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
				public async Task Returns_Result_From_FFalse()
				{
					// Arrange
					var value = Rnd.Str;
					var input = M.Wrap(value);
					var falseValue = Rnd.Int;
					var (fTest, fTrue, fFalse) = Setup(false, Rnd.Int, falseValue);

					// Act
					var r0 = await input.IfAsync(fTest, x => Task.FromResult(fTrue(x)), x => Task.FromResult(fFalse(x)));
					var r1 = await input.AsTask().IfAsync(fTest, fTrue, fFalse);
					var r2 = await input.AsTask().IfAsync(fTest, x => Task.FromResult(fTrue(x)), x => Task.FromResult(fFalse(x)));

					// Assert
					r0.AssertSome(falseValue);
					r1.AssertSome(falseValue);
					r2.AssertSome(falseValue);
				}

				[Fact]
				public async Task Calls_FTest_And_FFalse()
				{
					// Arrange
					var value = Rnd.Str;
					var input = M.Wrap(value);
					var (fTest, fTrue, fFalse) = Setup(false, Rnd.Int, Rnd.Int);

					// Act
					_ = await input.IfAsync(fTest, x => Task.FromResult(fTrue(x)), x => Task.FromResult(fFalse(x)));
					_ = await input.AsTask().IfAsync(fTest, fTrue, fFalse);
					_ = await input.AsTask().IfAsync(fTest, x => Task.FromResult(fTrue(x)), x => Task.FromResult(fFalse(x)));

					// Assert
					fTest.Received(3).Invoke(value);
					fFalse.Received(3).Invoke(value);
					fTrue.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
				}
			}

			public class Predicate_Returns_True
			{
				[Fact]
				public async Task Returns_Result_From_FTrue()
				{
					// Arrange
					var value = Rnd.Str;
					var input = M.Wrap(value);
					var trueValue = Rnd.Int;
					var (fTest, fTrue, fFalse) = Setup(true, trueValue, Rnd.Int);

					// Act
					var r0 = await input.IfAsync(fTest, x => Task.FromResult(fTrue(x)), x => Task.FromResult(fFalse(x)));
					var r1 = await input.AsTask().IfAsync(fTest, fTrue, fFalse);
					var r2 = await input.AsTask().IfAsync(fTest, x => Task.FromResult(fTrue(x)), x => Task.FromResult(fFalse(x)));

					// Assert
					r0.AssertSome(trueValue);
					r1.AssertSome(trueValue);
					r2.AssertSome(trueValue);
				}

				[Fact]
				public async Task Calls_FTest_And_FTrue()
				{
					// Arrange
					var value = Rnd.Str;
					var input = M.Wrap(value);
					var (fTest, fTrue, fFalse) = Setup(true, Rnd.Int, Rnd.Int);

					// Act
					_ = await input.IfAsync(fTest, x => Task.FromResult(fTrue(x)), x => Task.FromResult(fFalse(x)));
					_ = await input.AsTask().IfAsync(fTest, fTrue, fFalse);
					_ = await input.AsTask().IfAsync(fTest, x => Task.FromResult(fTrue(x)), x => Task.FromResult(fFalse(x)));

					// Assert
					fTest.Received(3).Invoke(value);
					fTrue.Received(3).Invoke(value);
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
				public async Task Returns_None()
				{
					// Arrange
					var input = NoneGen.Create<string>();
					var (fTest, fThen) = SetupSameType(false, Rnd.Str);

					// Act
					var r0 = await input.IfAsync(fTest, x => Task.FromResult(fThen(x)));
					var r1 = await input.AsTask().IfAsync(fTest, fThen);
					var r2 = await input.AsTask().IfAsync(fTest, x => Task.FromResult(fThen(x)));

					// Assert
					r0.AssertNone();
					r1.AssertNone();
					r2.AssertNone();
				}

				[Fact]
				public async Task Functions_Are_Not_Invoked()
				{
					// Arrange
					var input = NoneGen.Create<string>();
					var (fTest, fThen) = SetupSameType(false, Rnd.Str);

					// Act
					_ = await input.IfAsync(fTest, x => Task.FromResult(fThen(x)));
					_ = await input.AsTask().IfAsync(fTest, fThen);
					_ = await input.AsTask().IfAsync(fTest, x => Task.FromResult(fThen(x)));

					// Assert
					fTest.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
					fThen.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
				}
			}

			public class Predicate_Returns_True
			{
				[Fact]
				public async Task Returns_None()
				{
					// Arrange
					var input = NoneGen.Create<string>();
					var (fTest, fThen) = SetupSameType(true, Rnd.Str);

					// Act
					var r0 = await input.IfAsync(fTest, x => Task.FromResult(fThen(x)));
					var r1 = await input.AsTask().IfAsync(fTest, fThen);
					var r2 = await input.AsTask().IfAsync(fTest, x => Task.FromResult(fThen(x)));

					// Assert
					r0.AssertNone();
					r1.AssertNone();
					r2.AssertNone();
				}

				[Fact]
				public async Task Functions_Are_Not_Invoked()
				{
					// Arrange
					var input = NoneGen.Create<string>();
					var (fTest, fThen) = SetupSameType(true, Rnd.Str);

					// Act
					_ = await input.IfAsync(fTest, x => Task.FromResult(fThen(x)));
					_ = await input.AsTask().IfAsync(fTest, fThen);
					_ = await input.AsTask().IfAsync(fTest, x => Task.FromResult(fThen(x)));

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
				public async Task Returns_Original_Value()
				{
					// Arrange
					var value = Rnd.Str;
					var input = M.Wrap(value);
					var (fTest, fThen) = SetupSameType(false, Rnd.Str);

					// Act
					var r0 = await input.IfAsync(fTest, x => Task.FromResult(fThen(x)));
					var r1 = await input.AsTask().IfAsync(fTest, fThen);
					var r2 = await input.AsTask().IfAsync(fTest, x => Task.FromResult(fThen(x)));

					// Assert
					r0.AssertSome(value);
					r1.AssertSome(value);
					r2.AssertSome(value);
				}

				[Fact]
				public async Task Calls_FTest_Only()
				{
					// Arrange
					var value = Rnd.Str;
					var input = M.Wrap(value);
					var (fTest, fThen) = SetupSameType(false, Rnd.Str);

					// Act
					_ = await input.IfAsync(fTest, x => Task.FromResult(fThen(x)));
					_ = await input.AsTask().IfAsync(fTest, fThen);
					_ = await input.AsTask().IfAsync(fTest, x => Task.FromResult(fThen(x)));

					// Assert
					fTest.Received(3).Invoke(value);
					fThen.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
				}
			}

			public class Predicate_Returns_True
			{
				[Fact]
				public async Task Returns_Result_From_FThen()
				{
					// Arrange
					var value = Rnd.Str;
					var input = M.Wrap(value);
					var thenValue = Rnd.Str;
					var (fTest, fThen) = SetupSameType(true, thenValue);

					// Act
					var r0 = await input.IfAsync(fTest, x => Task.FromResult(fThen(x)));
					var r1 = await input.AsTask().IfAsync(fTest, fThen);
					var r2 = await input.AsTask().IfAsync(fTest, x => Task.FromResult(fThen(x)));

					// Assert
					r0.AssertSome(thenValue);
					r1.AssertSome(thenValue);
					r2.AssertSome(thenValue);
				}

				[Fact]
				public async Task Calls_FTest_And_FThen()
				{
					// Arrange
					var value = Rnd.Str;
					var input = M.Wrap(value);
					var (fTest, fThen) = SetupSameType(true, Rnd.Str);

					// Act
					_ = await input.IfAsync(fTest, x => Task.FromResult(fThen(x)));
					_ = await input.AsTask().IfAsync(fTest, fThen);
					_ = await input.AsTask().IfAsync(fTest, x => Task.FromResult(fThen(x)));

					// Assert
					fTest.Received(3).Invoke(value);
					fThen.Received(3).Invoke(value);
				}
			}
		}
	}
}
