// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class IfNotAsync_Tests
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
			public async Task Returns_None()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var (fTest, fThen) = Setup(false, Rnd.Str);

				// Act
				var r0 = await input.IfNotAsync(fTest, async x => fThen(x));
				var r1 = await input.AsTask().IfNotAsync(fTest, fThen);
				var r2 = await input.AsTask().IfNotAsync(fTest, async x => fThen(x));

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
				var (fTest, fThen) = Setup(false, Rnd.Str);

				// Act
				_ = await input.IfNotAsync(fTest, async x => fThen(x));
				_ = await input.AsTask().IfNotAsync(fTest, fThen);
				_ = await input.AsTask().IfNotAsync(fTest, async x => fThen(x));

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
				var (fTest, fThen) = Setup(true, Rnd.Str);

				// Act
				var r0 = await input.IfNotAsync(fTest, async x => fThen(x));
				var r1 = await input.AsTask().IfNotAsync(fTest, fThen);
				var r2 = await input.AsTask().IfNotAsync(fTest, async x => fThen(x));

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
				var (fTest, fThen) = Setup(true, Rnd.Str);

				// Act
				_ = await input.IfNotAsync(fTest, async x => fThen(x));
				_ = await input.AsTask().IfNotAsync(fTest, fThen);
				_ = await input.AsTask().IfNotAsync(fTest, async x => fThen(x));

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
			public async Task Returns_Result_From_FThen()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var thenValue = Rnd.Str;
				var (fTest, fThen) = Setup(false, thenValue);

				// Act
				var r0 = await input.IfNotAsync(fTest, async x => fThen(x));
				var r1 = await input.AsTask().IfNotAsync(fTest, fThen);
				var r2 = await input.AsTask().IfNotAsync(fTest, async x => fThen(x));

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
				var (fTest, fThen) = Setup(false, Rnd.Str);

				// Act
				_ = await input.IfNotAsync(fTest, async x => fThen(x));
				_ = await input.AsTask().IfNotAsync(fTest, fThen);
				_ = await input.AsTask().IfNotAsync(fTest, async x => fThen(x));

				// Assert
				fTest.Received(3).Invoke(value);
				fThen.Received(3).Invoke(value);
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public async Task Returns_Original_Value()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var (fTest, fThen) = Setup(true, Rnd.Str);

				// Act
				var r0 = await input.IfNotAsync(fTest, async x => fThen(x));
				var r1 = await input.AsTask().IfNotAsync(fTest, fThen);
				var r2 = await input.AsTask().IfNotAsync(fTest, async x => fThen(x));

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
				var (fTest, fThen) = Setup(true, Rnd.Str);

				// Act
				_ = await input.IfNotAsync(fTest, async x => fThen(x));
				_ = await input.AsTask().IfNotAsync(fTest, fThen);
				_ = await input.AsTask().IfNotAsync(fTest, async x => fThen(x));

				// Assert
				fTest.Received(3).Invoke(value);
				fThen.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			}
		}
	}
}
