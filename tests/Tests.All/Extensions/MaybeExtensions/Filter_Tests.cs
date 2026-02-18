// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class Filter_Tests
{
	public class With_None
	{
		[Fact]
		public void Does_Not_Call_FTest()
		{
			// Arrange
			var input = NoneGen.Create<string>();
			var fTest = Substitute.For<Func<string, bool>>();

			// Act
			_ = input.Filter(fTest);

			// Assert
			fTest.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
		}

		[Fact]
		public void Returns_None()
		{
			// Arrange
			var input = NoneGen.Create<string>();
			var fTest = Substitute.For<Func<string, bool>>();

			// Act
			var result = input.Filter(fTest);

			// Assert
			result.AssertNone();
		}
	}

	public class With_Some
	{
		public class Predicate_Returns_False
		{
			[Fact]
			public void Calls_FTest_With_Value()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var fTest = Substitute.For<Func<string, bool>>();
				fTest.Invoke(Arg.Any<string>()).Returns(false);

				// Act
				_ = input.Filter(fTest);

				// Assert
				fTest.Received(1).Invoke(value);
			}

			[Fact]
			public void Returns_None()
			{
				// Arrange
				var input = M.Wrap(Rnd.Str);
				var fTest = Substitute.For<Func<string, bool>>();
				fTest.Invoke(Arg.Any<string>()).Returns(false);

				// Act
				var result = input.Filter(fTest);

				// Assert
				result.AssertNone();
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public void Calls_FTest_With_Value()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var fTest = Substitute.For<Func<string, bool>>();
				fTest.Invoke(Arg.Any<string>()).Returns(true);

				// Act
				_ = input.Filter(fTest);

				// Assert
				fTest.Received(1).Invoke(value);
			}

			[Fact]
			public void Returns_Some_With_Original_Value()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var fTest = Substitute.For<Func<string, bool>>();
				fTest.Invoke(Arg.Any<string>()).Returns(true);

				// Act
				var result = input.Filter(fTest);

				// Assert
				result.AssertSome(value);
			}
		}
	}
}
