// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class IfOk_Tests
{
	public class With_Failure
	{
		[Fact]
		public void Returns_Failure()
		{
			// Arrange
			var value = Rnd.Str;
			var input = FailGen.Create<int>(new(value));
			var f = Substitute.For<Action<int>>();

			// Act
			var result = input.IfOk(f);

			// Assert
			result.AssertFailure(value);
		}

		[Fact]
		public void Function_Is_Not_Invoked()
		{
			// Arrange
			var input = FailGen.Create<int>();
			var f = Substitute.For<Action<int>>();

			// Act
			_ = input.IfOk(f);

			// Assert
			f.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
		}
	}

	public class With_Ok
	{
		[Fact]
		public void Invokes_Function_With_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Wrap(value);
			var f = Substitute.For<Action<int>>();

			// Act
			_ = input.IfOk(f);

			// Assert
			f.Received(1).Invoke(value);
		}

		[Fact]
		public void Returns_Ok()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Wrap(value);
			var f = Substitute.For<Action<int>>();

			// Act
			var result = input.IfOk(f);

			// Assert
			result.AssertOk(value);
		}
	}
}
