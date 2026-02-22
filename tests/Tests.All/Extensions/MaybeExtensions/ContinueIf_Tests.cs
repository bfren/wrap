// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class ContinueIf_Tests
{
	private static Func<string, bool> Setup(bool predicateReturn)
	{
		var fTest = Substitute.For<Func<string, bool>>();
		fTest.Invoke(Arg.Any<string>()).Returns(predicateReturn);

		return fTest;
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
				var fTest = Setup(false);

				// Act
				var result = input.ContinueIf(fTest);

				// Assert
				result.AssertNone();
			}

			[Fact]
			public void Function_Is_Not_Invoked()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var fTest = Setup(false);

				// Act
				_ = input.ContinueIf(fTest);

				// Assert
				fTest.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public void Returns_None()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var fTest = Setup(true);

				// Act
				var result = input.ContinueIf(fTest);

				// Assert
				result.AssertNone();
			}

			[Fact]
			public void Function_Is_Not_Invoked()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var fTest = Setup(true);

				// Act
				_ = input.ContinueIf(fTest);

				// Assert
				fTest.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			}
		}
	}

	public class With_Some
	{
		public class Predicate_Returns_False
		{
			[Fact]
			public void Returns_None()
			{
				// Arrange
				var input = M.Wrap(Rnd.Str);
				var fTest = Setup(false);

				// Act
				var result = input.ContinueIf(fTest);

				// Assert
				result.AssertNone();
			}

			[Fact]
			public void Function_Is_Invoked()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var fTest = Setup(false);

				// Act
				_ = input.ContinueIf(fTest);

				// Assert
				fTest.Received().Invoke(value);
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public void Returns_Original_Some()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var fTest = Setup(true);

				// Act
				var result = input.ContinueIf(fTest);

				// Assert
				result.AssertSome(value);
			}

			[Fact]
			public void Function_Is_Invoked()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var fTest = Setup(true);

				// Act
				_ = input.ContinueIf(fTest);

				// Assert
				fTest.Received().Invoke(value);
			}
		}
	}
}
