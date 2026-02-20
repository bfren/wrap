// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class MapIf_Tests
{
	private static (Func<string, bool> fTest, Func<string, int> f) Setup(bool predicateReturn)
	{
		var fTest = Substitute.For<Func<string, bool>>();
		fTest.Invoke(Arg.Any<string>()).Returns(predicateReturn);
		var f = Substitute.For<Func<string, int>>();
		return (fTest, f);
	}

	public class With_Failure
	{
		[Fact]
		public void Returns_Failure()
		{
			// Arrange
			var value = Rnd.Str;
			var input = FailGen.Create<string>(new(value));
			var (fTest, f) = Setup(true);

			// Act
			var result = input.MapIf(fTest, f);

			// Assert
			result.AssertFailure(value);
		}

		[Fact]
		public void f_Is_Not_Invoked()
		{
			// Arrange
			var input = FailGen.Create<string>();
			var (fTest, f) = Setup(true);

			// Act
			_ = input.MapIf(fTest, f);

			// Assert
			f.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
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
				var input = R.Wrap(Rnd.Str);
				var (fTest, f) = Setup(false);

				// Act
				var result = input.MapIf(fTest, f);

				// Assert
				result.AssertFailure(C.TestFalseMessage);
			}

			[Fact]
			public void f_Is_Not_Invoked()
			{
				// Arrange
				var input = R.Wrap(Rnd.Str);
				var (fTest, f) = Setup(false);

				// Act
				_ = input.MapIf(fTest, f);

				// Assert
				f.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public void Returns_Ok_With_Mapped_Value()
			{
				// Arrange
				var value = Rnd.Int;
				var input = R.Wrap(Rnd.Str);
				var (fTest, f) = Setup(true);
				f.Invoke(Arg.Any<string>()).Returns(value);

				// Act
				var result = input.MapIf(fTest, f);

				// Assert
				result.AssertOk(value);
			}
		}
	}
}
