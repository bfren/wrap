// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class Map_Tests
{
	private static Func<string, int> Setup(int returnValue)
	{
		var f = Substitute.For<Func<string, int>>();
		f.Invoke(Arg.Any<string>()).Returns(returnValue);
		return f;
	}

	public class With_None
	{
		[Fact]
		public void Does_Not_Call_F()
		{
			// Arrange
			var input = NoneGen.Create<string>();
			var f = Setup(Rnd.Int);

			// Act
			_ = input.Map(f);

			// Assert
			f.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
		}

		[Fact]
		public void Returns_None()
		{
			// Arrange
			var input = NoneGen.Create<string>();
			var f = Setup(Rnd.Int);

			// Act
			var result = input.Map(f);

			// Assert
			result.AssertNone();
		}
	}

	public class With_Some
	{
		[Fact]
		public void Calls_F_With_Value()
		{
			// Arrange
			var value = Rnd.Str;
			var input = M.Wrap(value);
			var f = Setup(Rnd.Int);

			// Act
			_ = input.Map(f);

			// Assert
			f.Received().Invoke(value);
		}

		[Fact]
		public void Returns_Mapped_Value()
		{
			// Arrange
			var value = Rnd.Str;
			var input = M.Wrap(value);
			var mappedValue = Rnd.Int;
			var f = Setup(mappedValue);

			// Act
			var result = input.Map(f);

			// Assert
			result.AssertSome(mappedValue);
		}
	}
}
