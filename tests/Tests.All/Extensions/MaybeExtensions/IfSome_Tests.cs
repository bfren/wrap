// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class IfSome_Tests
{
	public class With_None
	{
		[Fact]
		public void Does_Not_Call_F()
		{
			// Arrange
			var input = NoneGen.Create<string>();
			var f = Substitute.For<Action<string>>();

			// Act
			_ = input.IfSome(f);

			// Assert
			f.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
		}

		[Fact]
		public void Returns_None()
		{
			// Arrange
			var input = NoneGen.Create<string>();
			var f = Substitute.For<Action<string>>();

			// Act
			var result = input.IfSome(f);

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
			var f = Substitute.For<Action<string>>();

			// Act
			_ = input.IfSome(f);

			// Assert
			f.Received().Invoke(value);
		}

		[Fact]
		public void Returns_Original_Value()
		{
			// Arrange
			var value = Rnd.Str;
			var input = M.Wrap(value);
			var f = Substitute.For<Action<string>>();

			// Act
			var result = input.IfSome(f);

			// Assert
			result.AssertSome(value);
		}
	}
}
