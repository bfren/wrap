// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class IfFailed_Tests
{
	public class With_Ok
	{
		[Fact]
		public void Returns_Ok()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Wrap(value);
			var f = Substitute.For<Action<Failure>>();

			// Act
			var result = input.IfFailed(f);

			// Assert
			result.AssertOk(value);
		}

		[Fact]
		public void Function_Is_Not_Invoked()
		{
			// Arrange
			var input = R.Wrap(Rnd.Int);
			var f = Substitute.For<Action<Failure>>();

			// Act
			_ = input.IfFailed(f);

			// Assert
			f.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<Failure>());
		}
	}

	public class With_Failure
	{
		[Fact]
		public void Invokes_Function_With_Failure_Value()
		{
			// Arrange
			var input = FailGen.Create<int>();
			var f = Substitute.For<Action<Failure>>();

			// Act
			_ = input.IfFailed(f);

			// Assert
			f.Received(1).Invoke(Arg.Any<Failure>());
		}

		[Fact]
		public void Returns_Failure()
		{
			// Arrange
			var value = Rnd.Str;
			var input = FailGen.Create<int>(R.Fail(value));
			var f = Substitute.For<Action<Failure>>();

			// Act
			var result = input.IfFailed(f);

			// Assert
			result.AssertFailure(value);
		}
	}
}
