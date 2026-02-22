// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class MatchIfAsync_Tests
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

	// Calls all 30 async overloads and returns each result.
	// Group 1 ( 1- 3): Maybe<T>,       sync fNone, sync fTest, varying fFalse/fTrue async
	// Group 2 ( 4- 7): Maybe<T>,       sync fNone, async fTest, varying fFalse/fTrue async
	// Group 3 ( 8-11): Task<Maybe<T>>, sync fNone, sync fTest, varying fFalse/fTrue async
	// Group 4 (12-14): Task<Maybe<T>>, sync fNone, async fTest, varying fFalse/fTrue async
	// Group 5 (15-18): Maybe<T>,       async fNone, sync fTest, varying fFalse/fTrue async
	// Group 6 (19-22): Maybe<T>,       async fNone, async fTest, varying fFalse/fTrue async
	// Group 7 (23-26): Task<Maybe<T>>, async fNone, sync fTest, varying fFalse/fTrue async
	// Group 8 (27-30): Task<Maybe<T>>, async fNone, async fTest, varying fFalse/fTrue async
	private static async Task<IReadOnlyList<string>> CallAll(
		Maybe<int> input,
		Func<string> fNone,
		Func<int, bool> fTest,
		Func<int, string> fFalse,
		Func<int, string> fTrue)
	{
		var results = new List<string>
		{
			// Group 1: Maybe<T>, sync fNone, sync fTest
			await input.MatchIfAsync(fNone, fTest, async x => fFalse(x), fTrue),
			await input.MatchIfAsync(fNone, fTest, fFalse, async x => fTrue(x)),
			await input.MatchIfAsync(fNone, fTest, async x => fFalse(x), async x => fTrue(x)),

			// Group 2: Maybe<T>, sync fNone, async fTest
			await input.MatchIfAsync(fNone, async x => fTest(x), fFalse, fTrue),
			await input.MatchIfAsync(fNone, async x => fTest(x), async x => fFalse(x), fTrue),
			await input.MatchIfAsync(fNone, async x => fTest(x), fFalse, async x => fTrue(x)),
			await input.MatchIfAsync(fNone, async x => fTest(x), async x => fFalse(x), async x => fTrue(x)),

			// Group 3: Task<Maybe<T>>, sync fNone, sync fTest
			await input.AsTask().MatchIfAsync(fNone, fTest, fFalse, fTrue),
			await input.AsTask().MatchIfAsync(fNone, fTest, async x => fFalse(x), fTrue),
			await input.AsTask().MatchIfAsync(fNone, fTest, fFalse, async x => fTrue(x)),
			await input.AsTask().MatchIfAsync(fNone, fTest, async x => fFalse(x), async x => fTrue(x)),

			// Group 4: Task<Maybe<T>>, sync fNone, async fTest
			await input.AsTask().MatchIfAsync(fNone, async x => fTest(x), fFalse, fTrue),
			await input.AsTask().MatchIfAsync(fNone, async x => fTest(x), async x => fFalse(x), fTrue),
			await input.AsTask().MatchIfAsync(fNone, async x => fTest(x), fFalse, async x => fTrue(x)),

			// Group 5: Maybe<T>, async fNone, sync fTest
			await input.MatchIfAsync(async () => fNone(), fTest, fFalse, fTrue),
			await input.MatchIfAsync(async () => fNone(), fTest, async x => fFalse(x), fTrue),
			await input.MatchIfAsync(async () => fNone(), fTest, fFalse, async x => fTrue(x)),
			await input.MatchIfAsync(async () => fNone(), fTest, async x => fFalse(x), async x => fTrue(x)),

			// Group 6: Maybe<T>, async fNone, async fTest
			await input.MatchIfAsync(async () => fNone(), async x => fTest(x), fFalse, fTrue),
			await input.MatchIfAsync(async () => fNone(), async x => fTest(x), async x => fFalse(x), fTrue),
			await input.MatchIfAsync(async () => fNone(), async x => fTest(x), fFalse, async x => fTrue(x)),
			await input.MatchIfAsync(async () => fNone(), async x => fTest(x), async x => fFalse(x), async x => fTrue(x)),

			// Group 7: Task<Maybe<T>>, async fNone, sync fTest
			await input.AsTask().MatchIfAsync(async () => fNone(), fTest, fFalse, fTrue),
			await input.AsTask().MatchIfAsync(async () => fNone(), fTest, async x => fFalse(x), fTrue),
			await input.AsTask().MatchIfAsync(async () => fNone(), fTest, fFalse, async x => fTrue(x)),
			await input.AsTask().MatchIfAsync(async () => fNone(), fTest, async x => fFalse(x), async x => fTrue(x)),

			// Group 8: Task<Maybe<T>>, async fNone, async fTest
			await input.AsTask().MatchIfAsync(async () => fNone(), async x => fTest(x), fFalse, fTrue),
			await input.AsTask().MatchIfAsync(async () => fNone(), async x => fTest(x), async x => fFalse(x), fTrue),
			await input.AsTask().MatchIfAsync(async () => fNone(), async x => fTest(x), fFalse, async x => fTrue(x)),
			await input.AsTask().MatchIfAsync(async () => fNone(), async x => fTest(x), async x => fFalse(x), async x => fTrue(x))
		};

		return results;
	}

	public class With_None
	{
		[Fact]
		public async Task Calls_FNone_And_Does_Not_Call_Others()
		{
			// Arrange
			Maybe<int> input = M.None;
			var (fNone, fTest, fFalse, fTrue) = Setup(Rnd.Str, false, Rnd.Str, Rnd.Str);

			// Act
			_ = await CallAll(input, fNone, fTest, fFalse, fTrue);

			// Assert
			fNone.Received(30).Invoke();
			fTest.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			fFalse.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			fTrue.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
		}

		[Fact]
		public async Task Returns_FNone_Value()
		{
			// Arrange
			Maybe<int> input = M.None;
			var value = Rnd.Str;
			var (fNone, fTest, fFalse, fTrue) = Setup(value, false, Rnd.Str, Rnd.Str);

			// Act
			var results = await CallAll(input, fNone, fTest, fFalse, fTrue);

			// Assert
			Assert.All(results, r => Assert.Equal(value, r));
		}
	}

	public class With_Some
	{
		public class Predicate_Returns_False
		{
			[Fact]
			public async Task Calls_FTest_And_FFalse_Not_FTrue()
			{
				// Arrange
				var value = Rnd.Int;
				var input = M.Wrap(value);
				var (fNone, fTest, fFalse, fTrue) = Setup(Rnd.Str, false, Rnd.Str, Rnd.Str);

				// Act
				_ = await CallAll(input, fNone, fTest, fFalse, fTrue);

				// Assert
				fTest.Received(30).Invoke(value);
				fFalse.Received(30).Invoke(value);
				fTrue.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
				fNone.DidNotReceive().Invoke();
			}

			[Fact]
			public async Task Returns_FFalse_Value()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(Rnd.Int);
				var (fNone, fTest, fFalse, fTrue) = Setup(Rnd.Str, false, value, Rnd.Str);

				// Act
				var results = await CallAll(input, fNone, fTest, fFalse, fTrue);

				// Assert
				Assert.All(results, r => Assert.Equal(value, r));
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public async Task Calls_FTest_And_FTrue_Not_FFalse()
			{
				// Arrange
				var value = Rnd.Int;
				var input = M.Wrap(value);
				var (fNone, fTest, fFalse, fTrue) = Setup(Rnd.Str, true, Rnd.Str, Rnd.Str);

				// Act
				_ = await CallAll(input, fNone, fTest, fFalse, fTrue);

				// Assert
				fTest.Received(30).Invoke(value);
				fTrue.Received(30).Invoke(value);
				fFalse.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
				fNone.DidNotReceive().Invoke();
			}

			[Fact]
			public async Task Returns_FTrue_Value()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(Rnd.Int);
				var (fNone, fTest, fFalse, fTrue) = Setup(Rnd.Str, true, Rnd.Str, value);

				// Act
				var results = await CallAll(input, fNone, fTest, fFalse, fTrue);

				// Assert
				Assert.All(results, r => Assert.Equal(value, r));
			}
		}
	}
}
