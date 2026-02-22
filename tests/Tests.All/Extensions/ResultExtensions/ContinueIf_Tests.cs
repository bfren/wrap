// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class ContinueIf_Tests
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
		public void Returns_Failure()
		{
			// Arrange
			var value = Rnd.Str;
			var input = FailGen.Create<int>(new(value));
			var fTest = Setup(false);

			// Act
			var result = input.ContinueIf(fTest);

			// Assert
			result.AssertFailure(value);
		}

		[Fact]
		public void Test_Is_Not_Invoked()
		{
			// Arrange
			var input = FailGen.Create<int>();
			var fTest = Setup(false);

			// Act
			_ = input.ContinueIf(fTest);

			// Assert
			fTest.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
		}
	}

	public class With_Ok
	{
		public class Predicate_Returns_False
		{
			[Fact]
			public void Returns_Failure()
			{
				// Arrange
				var input = R.Wrap(Rnd.Int);
				var fTest = Setup(false);

				// Act
				var result = input.ContinueIf(fTest);

				// Assert
				result.AssertFailure(C.TestFalseMessage);
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public void Returns_Ok_With_Original_Value()
			{
				// Arrange
				var value = Rnd.Int;
				var input = R.Wrap(value);
				var fTest = Setup(true);

				// Act
				var result = input.ContinueIf(fTest);

				// Assert
				result.AssertOk(value);
			}
		}
	}
}
