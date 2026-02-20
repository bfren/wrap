// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class UnwrapAsync_Tests
{
	public class Throw_On_Failure
	{
		[Fact]
		public async Task With_Failure__Throws_Exception()
		{
			// Arrange
			var input = FailGen.Create<int>().AsTask();

			// Act / Assert
			await Assert.ThrowsAnyAsync<Exception>(() => input.UnwrapAsync());
		}

		[Fact]
		public async Task With_Ok__Returns_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Wrap(value).AsTask();

			// Act
			var result = await input.UnwrapAsync();

			// Assert
			Assert.Equal(value, result);
		}
	}

	public class Return_Value_On_Failure
	{
		[Fact]
		public async Task With_Failure__Calls_ifFailed()
		{
			// Arrange
			var defaultValue = Rnd.Int;
			var input = FailGen.Create<int>();

			// Act
			var r0 = await input.UnwrapAsync(async _ => defaultValue);
			var r1 = await input.AsTask().UnwrapAsync(_ => defaultValue);
			var r2 = await input.AsTask().UnwrapAsync(async _ => defaultValue);

			// Assert
			Assert.Equal(defaultValue, r0);
			Assert.Equal(defaultValue, r1);
			Assert.Equal(defaultValue, r2);
		}

		[Fact]
		public async Task With_Ok__Returns_Value()
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

	public class Handle_Failure_Then_Return
	{
		[Fact]
		public async Task With_Failure__Calls_ifFailed_Then_Returns_returnThis()
		{
			// Arrange
			var defaultValue = Rnd.Int;
			var handlerCalled = false;
			var input = FailGen.Create<int>();

			// Act
			var r0 = await input.UnwrapAsync(
				ifFailed: _ => { handlerCalled = true; return Task.CompletedTask; },
				returnThis: () => defaultValue
			);

			// Assert
			Assert.True(handlerCalled);
			Assert.Equal(defaultValue, r0);
		}

		[Fact]
		public async Task With_Failure__Calls_ifFailed_Then_Returns_returnThis_Async()
		{
			// Arrange
			var defaultValue = Rnd.Int;
			var handlerCalled = false;
			var input = FailGen.Create<int>();

			// Act
			var r0 = await input.UnwrapAsync(
				ifFailed: _ => { handlerCalled = true; return Task.CompletedTask; },
				returnThis: async () => defaultValue
			);

			// Assert
			Assert.True(handlerCalled);
			Assert.Equal(defaultValue, r0);
		}

		[Fact]
		public async Task With_Ok__Returns_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var handlerCalled = false;
			var input = R.Wrap(value);

			// Act
			var r0 = await input.UnwrapAsync(
				ifFailed: _ => { handlerCalled = true; return Task.CompletedTask; },
				returnThis: () => Rnd.Int
			);

			// Assert
			Assert.False(handlerCalled);
			Assert.Equal(value, r0);
		}
	}
}
