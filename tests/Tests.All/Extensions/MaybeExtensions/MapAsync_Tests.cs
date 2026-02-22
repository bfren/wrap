// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class MapAsync_Tests
{
	private static Func<string, Task<int>> SetupAsync(int value)
	{
		var f = Substitute.For<Func<string, Task<int>>>();
		f.Invoke(Arg.Any<string>()).Returns(Task.FromResult(value));
		return f;
	}

	private static Func<string, int> SetupSync(int value)
	{
		var f = Substitute.For<Func<string, int>>();
		f.Invoke(Arg.Any<string>()).Returns(value);
		return f;
	}

	public class With_None
	{
		[Fact]
		public async Task Does_Not_Call_F()
		{
			// Arrange
			var input = NoneGen.Create<string>();
			var fAsync = SetupAsync(Rnd.Int);
			var fSync = SetupSync(Rnd.Int);

			// Act
			_ = await input.MapAsync(fAsync);
			_ = await input.AsTask().MapAsync(fSync);
			_ = await input.AsTask().MapAsync(fAsync);

			// Assert
			await fAsync.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			fSync.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
		}

		[Fact]
		public async Task Returns_None()
		{
			// Arrange
			var input = NoneGen.Create<string>();
			var fAsync = SetupAsync(Rnd.Int);
			var fSync = SetupSync(Rnd.Int);

			// Act
			var r0 = await input.MapAsync(fAsync);
			var r1 = await input.AsTask().MapAsync(fSync);
			var r2 = await input.AsTask().MapAsync(fAsync);

			// Assert
			r0.AssertNone();
			r1.AssertNone();
			r2.AssertNone();
		}
	}

	public class With_Some
	{
		[Fact]
		public async Task Calls_F_With_Value()
		{
			// Arrange
			var value = Rnd.Str;
			var input = M.Wrap(value);
			var fAsync = SetupAsync(Rnd.Int);
			var fSync = SetupSync(Rnd.Int);

			// Act
			_ = await input.MapAsync(fAsync);
			_ = await input.AsTask().MapAsync(fSync);
			_ = await input.AsTask().MapAsync(fAsync);

			// Assert
			await fAsync.Received(2).Invoke(value);
			fSync.Received().Invoke(value);
		}

		[Fact]
		public async Task Returns_Mapped_Value()
		{
			// Arrange
			var value = Rnd.Str;
			var input = M.Wrap(value);
			var mappedValue = Rnd.Int;
			var fAsync = SetupAsync(mappedValue);
			var fSync = SetupSync(mappedValue);

			// Act
			var r0 = await input.MapAsync(fAsync);
			var r1 = await input.AsTask().MapAsync(fSync);
			var r2 = await input.AsTask().MapAsync(fAsync);

			// Assert
			r0.AssertSome(mappedValue);
			r1.AssertSome(mappedValue);
			r2.AssertSome(mappedValue);
		}
	}
}
