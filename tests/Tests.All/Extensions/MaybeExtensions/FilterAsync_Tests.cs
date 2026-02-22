// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class FilterAsync_Tests
{
	// Overloads:
	//   1. Maybe<T>,       async fTest
	//   2. Task<Maybe<T>>, sync  fTest
	//   3. Task<Maybe<T>>, async fTest

	public class With_None
	{
		[Fact]
		public async Task Does_Not_Call_FTest()
		{
			// Arrange
			var input = NoneGen.Create<string>();
			var fTest = Substitute.For<Func<string, bool>>();

			// Act
			_ = await input.FilterAsync(async x => fTest(x));
			_ = await input.AsTask().FilterAsync(fTest);
			_ = await input.AsTask().FilterAsync(async x => fTest(x));

			// Assert
			fTest.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
		}

		[Fact]
		public async Task Returns_None()
		{
			// Arrange
			var input = NoneGen.Create<string>();
			var fTest = Substitute.For<Func<string, bool>>();

			// Act
			var r0 = await input.FilterAsync(async x => fTest(x));
			var r1 = await input.AsTask().FilterAsync(fTest);
			var r2 = await input.AsTask().FilterAsync(async x => fTest(x));

			// Assert
			r0.AssertNone();
			r1.AssertNone();
			r2.AssertNone();
		}
	}

	public class With_Some
	{
		public class Predicate_Returns_False
		{
			[Fact]
			public async Task Calls_FTest_With_Value()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var fTest = Substitute.For<Func<string, bool>>();
				fTest.Invoke(Arg.Any<string>()).Returns(false);

				// Act
				_ = await input.FilterAsync(async x => fTest(x));
				_ = await input.AsTask().FilterAsync(fTest);
				_ = await input.AsTask().FilterAsync(async x => fTest(x));

				// Assert
				fTest.Received(3).Invoke(value);
			}

			[Fact]
			public async Task Returns_None()
			{
				// Arrange
				var input = M.Wrap(Rnd.Str);
				var fTest = Substitute.For<Func<string, bool>>();
				fTest.Invoke(Arg.Any<string>()).Returns(false);

				// Act
				var r0 = await input.FilterAsync(async x => fTest(x));
				var r1 = await input.AsTask().FilterAsync(fTest);
				var r2 = await input.AsTask().FilterAsync(async x => fTest(x));

				// Assert
				r0.AssertNone();
				r1.AssertNone();
				r2.AssertNone();
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public async Task Calls_FTest_With_Value()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var fTest = Substitute.For<Func<string, bool>>();
				fTest.Invoke(Arg.Any<string>()).Returns(true);

				// Act
				_ = await input.FilterAsync(async x => fTest(x));
				_ = await input.AsTask().FilterAsync(fTest);
				_ = await input.AsTask().FilterAsync(async x => fTest(x));

				// Assert
				fTest.Received(3).Invoke(value);
			}

			[Fact]
			public async Task Returns_Some_With_Original_Value()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var fTest = Substitute.For<Func<string, bool>>();
				fTest.Invoke(Arg.Any<string>()).Returns(true);

				// Act
				var r0 = await input.FilterAsync(async x => fTest(x));
				var r1 = await input.AsTask().FilterAsync(fTest);
				var r2 = await input.AsTask().FilterAsync(async x => fTest(x));

				// Assert
				r0.AssertSome(value);
				r1.AssertSome(value);
				r2.AssertSome(value);
			}
		}
	}
}
