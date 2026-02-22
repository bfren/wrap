// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class FilterAsync_Tests
{
	private static Func<int, bool> Setup(bool predicateReturn)
	{
		var fTest = Substitute.For<Func<int, bool>>();
		fTest.Invoke(Arg.Any<int>()).Returns(predicateReturn);
		return fTest;
	}

	public class With_Failure
	{
		[Fact]
		public async Task Returns_Failure()
		{
			// Arrange
			var value = Rnd.Str;
			var input = FailGen.Create<int>(new(value));
			var fTest = Setup(false);

			// Act
			var r0 = await input.FilterAsync(async x => fTest(x));
			var r1 = await input.AsTask().FilterAsync(fTest);
			var r2 = await input.AsTask().FilterAsync(async x => fTest(x));

			// Assert
			r0.AssertFailure(value);
			r1.AssertFailure(value);
			r2.AssertFailure(value);
		}

		[Fact]
		public async Task Test_Is_Not_Invoked()
		{
			// Arrange
			var input = FailGen.Create<int>();
			var fTest = Setup(false);

			// Act
			_ = await input.FilterAsync(async x => fTest(x));
			_ = await input.AsTask().FilterAsync(fTest);
			_ = await input.AsTask().FilterAsync(async x => fTest(x));

			// Assert
			fTest.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
		}
	}

	public class With_Ok
	{
		public class Predicate_Returns_False
		{
			[Fact]
			public async Task Returns_Failure()
			{
				// Arrange
				var input = R.Wrap(Rnd.Int);
				var fTest = Setup(false);

				// Act
				var r0 = await input.FilterAsync(async x => fTest(x));
				var r1 = await input.AsTask().FilterAsync(fTest);
				var r2 = await input.AsTask().FilterAsync(async x => fTest(x));

				// Assert
				r0.AssertFailure(C.TestFalseMessage);
				r1.AssertFailure(C.TestFalseMessage);
				r2.AssertFailure(C.TestFalseMessage);
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public async Task Returns_Ok_With_Value()
			{
				// Arrange
				var value = Rnd.Int;
				var input = R.Wrap(value);
				var fTest = Setup(true);

				// Act
				var r0 = await input.FilterAsync(async x => fTest(x));
				var r1 = await input.AsTask().FilterAsync(fTest);
				var r2 = await input.AsTask().FilterAsync(async x => fTest(x));

				// Assert
				r0.AssertOk(value);
				r1.AssertOk(value);
				r2.AssertOk(value);
				fTest.Received(3).Invoke(value);
			}
		}
	}
}
