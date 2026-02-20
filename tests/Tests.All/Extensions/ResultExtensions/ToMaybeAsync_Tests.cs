// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class ToMaybeAsync_Tests
{
	public class With_Failure
	{
		[Fact]
		public async Task Calls_Handler()
		{
			// Arrange
			var input = FailGen.Create<int>().AsTask();
			var handler = Substitute.For<Action<FailureValue>>();

			// Act
			_ = await input.ToMaybeAsync(handler);
			_ = await input.ToMaybeAsync(f => { handler(f); return M.None; });
			_ = await input.ToMaybeAsync(f => { handler(f); return Task.CompletedTask; });
			_ = await input.ToMaybeAsync(async f => { handler(f); return (Maybe<int>)M.None; });

			// Assert
			handler.ReceivedWithAnyArgs(4).Invoke(Arg.Any<FailureValue>());
		}

		[Fact]
		public async Task Returns_None()
		{
			// Arrange
			var input = FailGen.Create<int>().AsTask();

			// Act
			var r0 = await input.ToMaybeAsync(_ => { });
			var r1 = await input.ToMaybeAsync(_ => M.None);
			var r2 = await input.ToMaybeAsync(_ => Task.CompletedTask);
			var r3 = await input.ToMaybeAsync(async _ => (Maybe<int>)M.None);

			// Assert
			r0.AssertNone();
			r1.AssertNone();
			r2.AssertNone();
			r3.AssertNone();
		}
	}

	public class With_Ok
	{
		[Fact]
		public async Task Does_Not_Call_fFailureHandler()
		{
			// Arrange
			var input = R.Wrap(Rnd.Int).AsTask();
			var handler = Substitute.For<Action<FailureValue>>();

			// Act
			_ = await input.ToMaybeAsync(handler);
			_ = await input.ToMaybeAsync(f => { handler(f); return M.None; });

			// Assert
			handler.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<FailureValue>());
		}

		[Fact]
		public async Task Returns_Some_With_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Wrap(value).AsTask();

			// Act
			var r0 = await input.ToMaybeAsync(_ => { });
			var r1 = await input.ToMaybeAsync(_ => M.None);
			var r2 = await input.ToMaybeAsync(_ => Task.CompletedTask);
			var r3 = await input.ToMaybeAsync(async _ => (Maybe<int>)M.None);

			// Assert
			r0.AssertSome(value);
			r1.AssertSome(value);
			r2.AssertSome(value);
			r3.AssertSome(value);
		}
	}
}
