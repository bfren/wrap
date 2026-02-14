// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class UnwrapSingleAsync_Tests
{
	private static (Func<Task<int>> fNoneAsync, Func<int> fNoneSync) Setup(int defaultValue)
	{
		var fNoneAsync = Substitute.For<Func<Task<int>>>();
		fNoneAsync.Invoke().Returns(Task.FromResult(defaultValue));

		var fNoneSync = Substitute.For<Func<int>>();
		fNoneSync.Invoke().Returns(defaultValue);

		return (fNoneAsync, fNoneSync);
	}

	public class With_None
	{
		[Fact]
		public async Task Calls_FNone()
		{
			// Arrange
			var input = NoneGen.Create<List<int>>();
			var (fNoneAsync, fNoneSync) = Setup(Rnd.Int);

			// Act
			_ = await input.UnwrapSingleAsync(fNoneAsync);
			_ = await input.AsTask().UnwrapSingleAsync(fNoneSync);
			_ = await input.AsTask().UnwrapSingleAsync(fNoneAsync);

			// Assert
			await fNoneAsync.Received(2).Invoke();
			fNoneSync.Received().Invoke();
		}

		[Fact]
		public async Task Returns_Default_Value()
		{
			// Arrange
			var input = NoneGen.Create<List<int>>();
			var defaultValue = Rnd.Int;
			var (fNoneAsync, fNoneSync) = Setup(defaultValue);

			// Act
			var r0 = await input.UnwrapSingleAsync(fNoneAsync);
			var r1 = await input.AsTask().UnwrapSingleAsync(fNoneSync);
			var r2 = await input.AsTask().UnwrapSingleAsync(fNoneAsync);

			// Assert
			Assert.Equal(defaultValue, r0);
			Assert.Equal(defaultValue, r1);
			Assert.Equal(defaultValue, r2);
		}
	}

	public class With_Some
	{
		[Fact]
		public async Task Single_Value__Returns_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var input = M.Wrap(new List<int> { value });
			var (fNoneAsync, fNoneSync) = Setup(Rnd.Int);

			// Act
			var r0 = await input.UnwrapSingleAsync(fNoneAsync);
			var r1 = await input.AsTask().UnwrapSingleAsync(fNoneSync);
			var r2 = await input.AsTask().UnwrapSingleAsync(fNoneAsync);

			// Assert
			Assert.Equal(value, r0);
			Assert.Equal(value, r1);
			Assert.Equal(value, r2);
		}

		[Fact]
		public async Task Single_Value__Does_Not_Call_FNone()
		{
			// Arrange
			var value = Rnd.Int;
			var input = M.Wrap(new List<int> { value });
			var (fNoneAsync, fNoneSync) = Setup(Rnd.Int);

			// Act
			_ = await input.UnwrapSingleAsync(fNoneAsync);
			_ = await input.AsTask().UnwrapSingleAsync(fNoneSync);
			_ = await input.AsTask().UnwrapSingleAsync(fNoneAsync);

			// Assert
			await fNoneAsync.DidNotReceive().Invoke();
			fNoneSync.DidNotReceive().Invoke();
		}

		[Fact]
		public async Task Multiple_Values__Throws_InvalidOperationException()
		{
			// Arrange
			var input = M.Wrap(new List<int> { Rnd.Int, Rnd.Int, Rnd.Int });
			var (fNoneAsync, fNoneSync) = Setup(Rnd.Int);

			// Act & Assert
			await Assert.ThrowsAsync<InvalidOperationException>(async () => await input.UnwrapSingleAsync(fNoneAsync));
			await Assert.ThrowsAsync<InvalidOperationException>(async () => await input.AsTask().UnwrapSingleAsync(fNoneSync));
			await Assert.ThrowsAsync<InvalidOperationException>(async () => await input.AsTask().UnwrapSingleAsync(fNoneAsync));
		}

		[Fact]
		public async Task No_Values__Throws_InvalidOperationException()
		{
			// Arrange
			var input = M.Wrap(new List<int>());
			var (fNoneAsync, fNoneSync) = Setup(Rnd.Int);

			// Act & Assert
			await Assert.ThrowsAsync<InvalidOperationException>(async () => await input.UnwrapSingleAsync(fNoneAsync));
			await Assert.ThrowsAsync<InvalidOperationException>(async () => await input.AsTask().UnwrapSingleAsync(fNoneSync));
			await Assert.ThrowsAsync<InvalidOperationException>(async () => await input.AsTask().UnwrapSingleAsync(fNoneAsync));
		}
	}
}
