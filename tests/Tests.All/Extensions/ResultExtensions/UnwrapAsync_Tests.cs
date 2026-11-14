// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class UnwrapAsync_Tests
{
	public class With_IfFailed
	{
		public class With_Failure
		{
			[Fact]
			public async Task Calls_ifFailed()
			{
				// Arrange
				var defaultValue = Rnd.Int;
				var input = R.Fail<int>(Rnd.Str);

				// Act
				var r0 = await input.UnwrapAsync(async _ => defaultValue);
				var r1 = await input.AsTask().UnwrapAsync(_ => defaultValue);
				var r2 = await input.AsTask().UnwrapAsync(async _ => defaultValue);

				// Assert
				Assert.Equal(defaultValue, r0);
				Assert.Equal(defaultValue, r1);
				Assert.Equal(defaultValue, r2);
			}
		}

		public class With_Ok
		{
			[Fact]
			public async Task Returns_Value()
			{
				// Arrange
				var value = Rnd.Int;
				var input = R.Wrap(value);

				// Act
				var r0 = await input.UnwrapAsync(async _ => Rnd.Int);
				var r1 = await input.AsTask().UnwrapAsync(_ => Rnd.Int);
				var r2 = await input.AsTask().UnwrapAsync(async _ => Rnd.Int);

				// Assert
				Assert.Equal(value, r0);
				Assert.Equal(value, r1);
				Assert.Equal(value, r2);
			}
		}
	}

	public class With_Handler
	{
		public class With_Failure
		{
			[Fact]
			public async Task Calls_ifFailed_Then_Returns_returnThis()
			{
				// Arrange
				var value = Rnd.Int;
				var handler = Substitute.For<Action<Failure>>();
				var input = R.Fail<int>(Rnd.Str);

				// Act
				var r0 = await input.UnwrapAsync(async f => handler(f), async () => value);
				var r1 = await input.UnwrapAsync(async f => handler(f), () => value);
				var r2 = await input.UnwrapAsync(handler, async () => value);
				var r3 = await input.AsTask().UnwrapAsync(handler, () => value);
				var r4 = await input.AsTask().UnwrapAsync(async f => handler(f), () => value);
				var r5 = await input.AsTask().UnwrapAsync(handler, async () => value);
				var r6 = await input.AsTask().UnwrapAsync(async f => handler(f), async () => value);

				// Assert
				handler.ReceivedWithAnyArgs().Invoke(Arg.Any<Failure>());
				Assert.Equal(value, r0);
				Assert.Equal(value, r1);
				Assert.Equal(value, r2);
				Assert.Equal(value, r3);
				Assert.Equal(value, r4);
				Assert.Equal(value, r5);
				Assert.Equal(value, r6);
			}
		}

		public class With_Ok
		{
			[Fact]
			public async Task Returns_Value()
			{
				// Arrange
				var value = Rnd.Int;
				var handler = Substitute.For<Action<Failure>>();
				var input = R.Wrap(value);

				// Act
				var r0 = await input.UnwrapAsync(async f => handler(f), async () => value);
				var r1 = await input.UnwrapAsync(async f => handler(f), () => value);
				var r2 = await input.UnwrapAsync(handler, async () => value);
				var r3 = await input.AsTask().UnwrapAsync(handler, () => value);
				var r4 = await input.AsTask().UnwrapAsync(async f => handler(f), () => value);
				var r5 = await input.AsTask().UnwrapAsync(handler, async () => value);
				var r6 = await input.AsTask().UnwrapAsync(async f => handler(f), async () => value);

				// Assert
				handler.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<Failure>());
				Assert.Equal(value, r0);
				Assert.Equal(value, r1);
				Assert.Equal(value, r2);
				Assert.Equal(value, r3);
				Assert.Equal(value, r4);
				Assert.Equal(value, r5);
				Assert.Equal(value, r6);
			}
		}
	}
}
