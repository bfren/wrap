// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class UnwrapAsync_Tests
{
	private static (Func<Task<string>> fNoneAsync, Func<string> fNoneSync) Setup(string defaultValue)
	{
		var fNoneAsync = Substitute.For<Func<Task<string>>>();
		fNoneAsync.Invoke().Returns(Task.FromResult(defaultValue));

		var fNoneSync = Substitute.For<Func<string>>();
		fNoneSync.Invoke().Returns(defaultValue);

		return (fNoneAsync, fNoneSync);
	}

	public class With_None
	{
		[Fact]
		public async Task Calls_FNone()
		{
			// Arrange
			var input = NoneGen.Create<string>();
			var (fNoneAsync, fNoneSync) = Setup(Rnd.Str);

			// Act
			_ = await input.UnwrapAsync(fNoneAsync);
			_ = await input.AsTask().UnwrapAsync(fNoneSync);
			_ = await input.AsTask().UnwrapAsync(fNoneAsync);

			// Assert
			await fNoneAsync.Received(2).Invoke();
			fNoneSync.Received().Invoke();
		}

		[Fact]
		public async Task Returns_Default_Value()
		{
			// Arrange
			var input = NoneGen.Create<string>();
			var defaultValue = Rnd.Str;
			var (fNoneAsync, fNoneSync) = Setup(defaultValue);

			// Act
			var r0 = await input.UnwrapAsync(fNoneAsync);
			var r1 = await input.AsTask().UnwrapAsync(fNoneSync);
			var r2 = await input.AsTask().UnwrapAsync(fNoneAsync);

			// Assert
			Assert.Equal(defaultValue, r0);
			Assert.Equal(defaultValue, r1);
			Assert.Equal(defaultValue, r2);
		}
	}

	public class With_Some
	{
		[Fact]
		public async Task Does_Not_Call_FNone()
		{
			// Arrange
			var value = Rnd.Str;
			var input = M.Wrap(value);
			var (fNoneAsync, fNoneSync) = Setup(Rnd.Str);

			// Act
			_ = await input.UnwrapAsync(fNoneAsync);
			_ = await input.AsTask().UnwrapAsync(fNoneSync);
			_ = await input.AsTask().UnwrapAsync(fNoneAsync);

			// Assert
			await fNoneAsync.DidNotReceive().Invoke();
			fNoneSync.DidNotReceive().Invoke();
		}

		[Fact]
		public async Task Returns_Value()
		{
			// Arrange
			var value = Rnd.Str;
			var input = M.Wrap(value);
			var (fNoneAsync, fNoneSync) = Setup(Rnd.Str);

			// Act
			var r0 = await input.UnwrapAsync(fNoneAsync);
			var r1 = await input.AsTask().UnwrapAsync(fNoneSync);
			var r2 = await input.AsTask().UnwrapAsync(fNoneAsync);

			// Assert
			Assert.Equal(value, r0);
			Assert.Equal(value, r1);
			Assert.Equal(value, r2);
		}
	}
}
