// Wrap: Unit Tests
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
			var source = R.Fail<int>(Rnd.Str);
			var input = source.AsTask();
			var handler = Substitute.For<Action<Failure>>();

			// Act
			_ = await input.ToMaybeAsync(handler);
			_ = await input.ToMaybeAsync(f => { handler(f); return M.None; });
			_ = await input.ToMaybeAsync(f => { handler(f); return Task.CompletedTask; });
			_ = await input.ToMaybeAsync(async f => { handler(f); return M.None; });
			_ = await source.ToMaybeAsync(f => { handler(f); return Task.CompletedTask; });
			_ = await source.ToMaybeAsync(async f => { handler(f); return M.None; });

			// Assert
			handler.ReceivedWithAnyArgs(6).Invoke(Arg.Any<Failure>());
		}

		[Fact]
		public async Task Returns_None()
		{
			// Arrange
			var source = R.Fail<int>(Rnd.Str);
			var input = source.AsTask();

			// Act
			var r0 = await input.ToMaybeAsync(_ => { });
			var r1 = await input.ToMaybeAsync(_ => M.None);
			var r2 = await input.ToMaybeAsync(_ => Task.CompletedTask);
			var r3 = await input.ToMaybeAsync(async _ => M.None);
			var r4 = await source.ToMaybeAsync(_ => Task.CompletedTask);
			var r5 = await source.ToMaybeAsync(async _ => M.None);

			// Assert
			r0.AssertNone();
			r1.AssertNone();
			r2.AssertNone();
			r3.AssertNone();
			r4.AssertNone();
			r5.AssertNone();
		}
	}

	public class With_Ok
	{
		[Fact]
		public async Task Does_Not_Call_fFailureHandler()
		{
			// Arrange
			var source = R.Wrap(Rnd.Int);
			var input = source.AsTask();
			var handler = Substitute.For<Action<Failure>>();

			// Act
			_ = await input.ToMaybeAsync(handler);
			_ = await input.ToMaybeAsync(f => { handler(f); return M.None; });

			// Assert
			handler.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<Failure>());
		}

		[Fact]
		public async Task Returns_Some_With_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var source = R.Wrap(value);
			var input = source.AsTask();

			// Act
			var r0 = await input.ToMaybeAsync(_ => { });
			var r1 = await input.ToMaybeAsync(_ => M.None);
			var r2 = await input.ToMaybeAsync(_ => Task.CompletedTask);
			var r3 = await input.ToMaybeAsync(async _ => M.None);
			var r4 = await source.ToMaybeAsync(_ => Task.CompletedTask);
			var r5 = await source.ToMaybeAsync(async _ => M.None);

			// Assert
			r0.AssertSome(value);
			r1.AssertSome(value);
			r2.AssertSome(value);
			r3.AssertSome(value);
			r4.AssertSome(value);
			r5.AssertSome(value);
		}
	}
}
