// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class ToMaybeAsync_Tests
{
	public class With_Failure
	{
		[Fact]
		public async Task Calls_fFailureHandler()
		{
			// Arrange
			var input = FailGen.Create<int>();
			var fFailureHandler = Substitute.For<Action<FailureValue>>();

			// Act
			_ = await input.AsTask().ToMaybeAsync(fFailureHandler);
			_ = await input.AsTask().ToMaybeAsync(f => { fFailureHandler(f); return M.None; });

			// Assert
			fFailureHandler.ReceivedWithAnyArgs(2).Invoke(Arg.Any<FailureValue>());
		}

		[Fact]
		public async Task Returns_None()
		{
			// Arrange
			var input = FailGen.Create<int>();

			// Act
			var r0 = await input.AsTask().ToMaybeAsync(_ => { });
			var r1 = await input.AsTask().ToMaybeAsync(_ => M.None);
			var r2 = await input.AsTask().ToMaybeAsync(_ => Task.CompletedTask);
			var r3 = await input.AsTask().ToMaybeAsync(async _ => (Maybe<int>)M.None);

			// Assert
			Assert.True(r0.IsNone);
			Assert.True(r1.IsNone);
			Assert.True(r2.IsNone);
			Assert.True(r3.IsNone);
		}
	}

	public class With_Ok
	{
		[Fact]
		public async Task Does_Not_Call_fFailureHandler()
		{
			// Arrange
			var input = R.Wrap(Rnd.Int);
			var fFailureHandler = Substitute.For<Action<FailureValue>>();

			// Act
			_ = await input.AsTask().ToMaybeAsync(fFailureHandler);
			_ = await input.AsTask().ToMaybeAsync(f => { fFailureHandler(f); return M.None; });

			// Assert
			fFailureHandler.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<FailureValue>());
		}

		[Fact]
		public async Task Returns_Some_With_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Wrap(value);

			// Act
			var r0 = await input.AsTask().ToMaybeAsync(_ => { });
			var r1 = await input.AsTask().ToMaybeAsync(_ => M.None);
			var r2 = await input.AsTask().ToMaybeAsync(_ => Task.CompletedTask);
			var r3 = await input.AsTask().ToMaybeAsync(async _ => (Maybe<int>)M.None);

			// Assert
			Assert.True(r0.IsSome);
			Assert.Equal(value, r0.Unwrap(() => default));
			Assert.True(r1.IsSome);
			Assert.Equal(value, r1.Unwrap(() => default));
			Assert.True(r2.IsSome);
			Assert.Equal(value, r2.Unwrap(() => default));
			Assert.True(r3.IsSome);
			Assert.Equal(value, r3.Unwrap(() => default));
		}
	}
}
