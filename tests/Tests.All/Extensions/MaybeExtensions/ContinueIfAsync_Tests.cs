// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class ContinueIfAsync_Tests
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
			public async Task Returns_None()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var fTest = Setup(false);

				// Act
				var result = await input.AsTask().ContinueIfAsync(fTest);

				// Assert
				result.AssertNone();
			}

			[Fact]
			public async Task Function_Is_Not_Invoked()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var fTest = Setup(false);

				// Act
				_ = await input.AsTask().ContinueIfAsync(fTest);

				// Assert
				fTest.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public async Task Returns_None()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var fTest = Setup(true);

				// Act
				var result = await input.AsTask().ContinueIfAsync(fTest);

				// Assert
				result.AssertNone();
			}

			[Fact]
			public async Task Function_Is_Not_Invoked()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var fTest = Setup(true);

				// Act
				_ = await input.AsTask().ContinueIfAsync(fTest);

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
			public async Task Returns_None()
			{
				// Arrange
				var input = M.Wrap(Rnd.Str);
				var fTest = Setup(false);

				// Act
				var result = await input.AsTask().ContinueIfAsync(fTest);

				// Assert
				result.AssertNone();
			}

			[Fact]
			public async Task Function_Is_Invoked()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var fTest = Setup(false);

				// Act
				_ = await input.AsTask().ContinueIfAsync(fTest);

				// Assert
				fTest.Received().Invoke(value);
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public async Task Returns_Original_Some()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var fTest = Setup(true);

				// Act
				var result = await input.AsTask().ContinueIfAsync(fTest);

				// Assert
				result.AssertSome(value);
			}

			[Fact]
			public async Task Function_Is_Invoked()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var fTest = Setup(true);

				// Act
				_ = await input.AsTask().ContinueIfAsync(fTest);

				// Assert
				fTest.Received().Invoke(value);
			}
		}
	}
}
