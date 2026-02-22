// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class IfNone_Tests
{
	public class With_None
	{
		[Fact]
		public void Calls_F()
		{
			// Arrange
			var input = NoneGen.Create<string>();
			var f = Substitute.For<Action>();

			// Act
			_ = input.IfNone(f);

			// Assert
			f.Received().Invoke();
		}

		[Fact]
		public void Returns_None()
		{
			// Arrange
			var input = NoneGen.Create<string>();
			var f = Substitute.For<Action>();

			// Act
			var result = input.IfNone(f);

			// Assert
			result.AssertNone();
		}
	}

	public class With_Some
	{
		[Fact]
		public void Does_Not_Call_F()
		{
			// Arrange
			var value = Rnd.Str;
			var input = M.Wrap(value);
			var f = Substitute.For<Action>();

			// Act
			_ = input.IfNone(f);

			// Assert
			f.DidNotReceive().Invoke();
		}

		[Fact]
		public void Returns_Original_Value()
		{
			// Arrange
			var value = Rnd.Str;
			var input = M.Wrap(value);
			var f = Substitute.For<Action>();

			// Act
			var result = input.IfNone(f);

			// Assert
			result.AssertSome(value);
		}
	}
}
