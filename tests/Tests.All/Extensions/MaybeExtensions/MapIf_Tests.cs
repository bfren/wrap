// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class MapIf_Tests
{
	private static (Func<string, bool> fTest, Func<string, int> f) Setup(bool predicateReturn, int mappedValue)
	{
		var fTest = Substitute.For<Func<string, bool>>();
		fTest.Invoke(Arg.Any<string>()).Returns(predicateReturn);

		var f = Substitute.For<Func<string, int>>();
		f.Invoke(Arg.Any<string>()).Returns(mappedValue);

		return (fTest, f);
	}

	public class With_None
	{
		[Fact]
		public void Predicate_Returns_False__Returns_None()
		{
			// Arrange
			var input = NoneGen.Create<string>();
			var (fTest, f) = Setup(false, Rnd.Int);

			// Act
			var result = input.MapIf(fTest, f);

			// Assert
			result.AssertNone();
		}

		[Fact]
		public void Predicate_Returns_True__Returns_None()
		{
			// Arrange
			var input = NoneGen.Create<string>();
			var (fTest, f) = Setup(true, Rnd.Int);

			// Act
			var result = input.MapIf(fTest, f);

			// Assert
			result.AssertNone();
		}

		[Fact]
		public void Functions_Are_Not_Invoked()
		{
			// Arrange
			var input = NoneGen.Create<string>();
			var (fTest, f) = Setup(false, Rnd.Int);

			// Act
			_ = input.MapIf(fTest, f);

			// Assert
			fTest.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			f.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
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
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var (fTest, f) = Setup(false, Rnd.Int);

				// Act
				var result = input.MapIf(fTest, f);

				// Assert
				result.AssertNone();
			}

			[Fact]
			public void Calls_FTest_Only()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var (fTest, f) = Setup(false, Rnd.Int);

				// Act
				_ = input.MapIf(fTest, f);

				// Assert
				fTest.Received().Invoke(value);
				f.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public void Returns_Mapped_Value()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var mappedValue = Rnd.Int;
				var (fTest, f) = Setup(true, mappedValue);

				// Act
				var result = input.MapIf(fTest, f);

				// Assert
				result.AssertSome(mappedValue);
			}

			[Fact]
			public void Calls_FTest_And_F()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var (fTest, f) = Setup(true, Rnd.Int);

				// Act
				_ = input.MapIf(fTest, f);

				// Assert
				fTest.Received().Invoke(value);
				f.Received().Invoke(value);
			}
		}
	}
}
