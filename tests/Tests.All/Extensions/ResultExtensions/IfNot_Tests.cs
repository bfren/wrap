// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class IfNot_Tests
{
	public class With_Failure
	{
		[Fact]
		public void Returns_Failure()
		{
			// Arrange
			var value = Rnd.Str;
			var input = FailGen.Create<int>(new(value));
			var fTest = Substitute.For<Func<int, bool>>();
			fTest.Invoke(Arg.Any<int>()).Returns(false);
			var fThen = Substitute.For<Func<int, Result<int>>>();

			// Act
			var result = input.IfNot(fTest, fThen);

			// Assert
			result.AssertFailure(value);
		}

		[Fact]
		public void fThen_Is_Not_Invoked()
		{
			// Arrange
			var input = FailGen.Create<int>();
			var fTest = Substitute.For<Func<int, bool>>();
			fTest.Invoke(Arg.Any<int>()).Returns(false);
			var fThen = Substitute.For<Func<int, Result<int>>>();

			// Act
			_ = input.IfNot(fTest, fThen);

			// Assert
			fThen.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
		}
	}

	public class With_Ok
	{
		public class Predicate_Returns_True
		{
			[Fact]
			public void Returns_Original_Value()
			{
				// Arrange
				var value = Rnd.Int;
				var input = R.Wrap(value);
				var fTest = Substitute.For<Func<int, bool>>();
				fTest.Invoke(Arg.Any<int>()).Returns(true);
				var fThen = Substitute.For<Func<int, Result<int>>>();

				// Act
				var result = input.IfNot(fTest, fThen);

				// Assert
				result.AssertOk(value);
			}

			[Fact]
			public void fThen_Is_Not_Invoked()
			{
				// Arrange
				var input = R.Wrap(Rnd.Int);
				var fTest = Substitute.For<Func<int, bool>>();
				fTest.Invoke(Arg.Any<int>()).Returns(true);
				var fThen = Substitute.For<Func<int, Result<int>>>();

				// Act
				_ = input.IfNot(fTest, fThen);

				// Assert
				fThen.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			}
		}

		public class Predicate_Returns_False
		{
			[Fact]
			public void Returns_fThen_Result()
			{
				// Arrange
				var value = Rnd.Int;
				var input = R.Wrap(Rnd.Int);
				var fTest = Substitute.For<Func<int, bool>>();
				fTest.Invoke(Arg.Any<int>()).Returns(false);
				var fThen = Substitute.For<Func<int, Result<int>>>();
				fThen.Invoke(Arg.Any<int>()).Returns(value);

				// Act
				var result = input.IfNot(fTest, fThen);

				// Assert
				result.AssertOk(value);
			}
		}
	}
}
