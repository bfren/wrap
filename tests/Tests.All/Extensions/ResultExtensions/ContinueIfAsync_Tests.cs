// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class ContinueIfAsync_Tests
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
			var result = await input.AsTask().ContinueIfAsync(fTest);

			// Assert
			result.AssertFailure(value);
		}

		[Fact]
		public async Task Test_Is_Not_Invoked()
		{
			// Arrange
			var input = FailGen.Create<int>();
			var fTest = Setup(false);

			// Act
			_ = await input.AsTask().ContinueIfAsync(fTest);

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
				var result = await input.AsTask().ContinueIfAsync(fTest);

				// Assert
				result.AssertFailure(C.TestFalseMessage);
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public async Task Returns_Ok_With_Original_Value()
			{
				// Arrange
				var value = Rnd.Int;
				var input = R.Wrap(value);
				var fTest = Setup(true);

				// Act
				var result = await input.AsTask().ContinueIfAsync(fTest);

				// Assert
				result.AssertOk(value);
			}
		}
	}
}
