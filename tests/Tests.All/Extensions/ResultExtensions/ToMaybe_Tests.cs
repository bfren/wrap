// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class ToMaybe_Tests
{
	public class With_Failure
	{
		[Fact]
		public void Calls_fFailureHandler()
		{
			// Arrange
			var input = FailGen.Create<int>();
			var fFailureHandler = Substitute.For<Action<FailureValue>>();

			// Act
			_ = input.ToMaybe(fFailureHandler);

			// Assert
			fFailureHandler.ReceivedWithAnyArgs(1).Invoke(Arg.Any<FailureValue>());
		}

		[Fact]
		public void Returns_None()
		{
			// Arrange
			var input = FailGen.Create<int>();

			// Act
			var result = input.ToMaybe(_ => { });

			// Assert
			Assert.True(result.IsNone);
		}
	}

	public class With_Ok
	{
		[Fact]
		public void Does_Not_Call_fFailureHandler()
		{
			// Arrange
			var input = R.Wrap(Rnd.Int);
			var fFailureHandler = Substitute.For<Action<FailureValue>>();

			// Act
			_ = input.ToMaybe(fFailureHandler);

			// Assert
			fFailureHandler.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<FailureValue>());
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
			Assert.True(result.IsSome);
			Assert.Equal(value, result.Unwrap(() => default));
		}
	}
}
