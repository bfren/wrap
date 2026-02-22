// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class ToMaybe_Tests
{
	public class With_Failure
	{
		[Fact]
		public void Calls_Handler()
		{
			// Arrange
			var input = FailGen.Create<int>();
			var handler = Substitute.For<Action<FailureValue>>();

			// Act
			_ = input.ToMaybe(handler);

			// Assert
			handler.ReceivedWithAnyArgs(1).Invoke(Arg.Any<FailureValue>());
		}

		[Fact]
		public void Returns_None()
		{
			// Arrange
			var input = FailGen.Create<int>();

			// Act
			var result = input.ToMaybe(_ => { });

			// Assert
			result.AssertNone();
		}
	}

	public class With_Ok
	{
		[Fact]
		public void Does_Not_Call_Handler()
		{
			// Arrange
			var input = R.Wrap(Rnd.Int);
			var handler = Substitute.For<Action<FailureValue>>();

			// Act
			_ = input.ToMaybe(handler);

			// Assert
			handler.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<FailureValue>());
		}

		[Fact]
		public void Returns_Some_With_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Wrap(value);

			// Act
			var result = input.ToMaybe(_ => { });

			// Assert
			result.AssertSome(value);
		}
	}
}
