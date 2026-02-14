// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class IfNoneAsync_Tests
{
	public class With_None
	{
		[Fact]
		public async Task Calls_F()
		{
			// Arrange
			var input = NoneGen.Create<string>();
			var f = Substitute.For<Func<Task>>();
			f.Invoke().Returns(Task.CompletedTask);

			// Act
			var r0 = await input.IfNoneAsync(f);
			var r1 = await input.AsTask().IfNoneAsync(() => { });
			var r2 = await input.AsTask().IfNoneAsync(f);

			// Assert
			await f.Received(2).Invoke();
		}

		[Fact]
		public async Task Returns_None()
		{
			// Arrange
			var input = NoneGen.Create<string>();
			var f = Substitute.For<Func<Task>>();
			f.Invoke().Returns(Task.CompletedTask);

			// Act
			var r0 = await input.IfNoneAsync(f);
			var r1 = await input.AsTask().IfNoneAsync(() => { });
			var r2 = await input.AsTask().IfNoneAsync(f);

			// Assert
			r0.AssertNone();
			r1.AssertNone();
			r2.AssertNone();
		}
	}

	public class With_Some
	{
		[Fact]
		public async Task Does_Not_Call_F()
		{
			// Arrange
			var value = Rnd.Str;
			var input = M.Wrap(value);
			var f = Substitute.For<Func<Task>>();
			f.Invoke().Returns(Task.CompletedTask);

			// Act
			_ = await input.IfNoneAsync(f);
			_ = await input.AsTask().IfNoneAsync(() => { });
			_ = await input.AsTask().IfNoneAsync(f);

			// Assert
			await f.DidNotReceive().Invoke();
		}

		[Fact]
		public async Task Returns_Original_Value()
		{
			// Arrange
			var value = Rnd.Str;
			var input = M.Wrap(value);
			var f = Substitute.For<Func<Task>>();
			f.Invoke().Returns(Task.CompletedTask);

			// Act
			var r0 = await input.IfNoneAsync(f);
			var r1 = await input.AsTask().IfNoneAsync(() => { });
			var r2 = await input.AsTask().IfNoneAsync(f);

			// Assert
			r0.AssertSome(value);
			r1.AssertSome(value);
			r2.AssertSome(value);
		}
	}
}
