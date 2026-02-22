// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using NSubstitute.ExceptionExtensions;

namespace Wrap.Extensions.ResultExtensions_Tests;

public class MapAsync_Tests
{
	public class With_Failure
	{
		[Fact]
		public async Task Returns_Failure()
		{
			// Arrange
			var value = Rnd.Str;
			var input = FailGen.Create<string>(new(value));
			var f = Substitute.For<Func<string, int>>();

			// Act
			var r0 = await input.MapAsync(async x => f(x));
			var r1 = await input.AsTask().MapAsync(f);
			var r2 = await input.AsTask().MapAsync(async x => f(x));

			// Assert
			r0.AssertFailure(value);
			r1.AssertFailure(value);
			r2.AssertFailure(value);
		}

		[Fact]
		public async Task f_Is_Not_Invoked()
		{
			// Arrange
			var input = FailGen.Create<string>();
			var f = Substitute.For<Func<string, int>>();

			// Act
			_ = await input.MapAsync(async x => f(x));
			_ = await input.AsTask().MapAsync(f);
			_ = await input.AsTask().MapAsync(async x => f(x));

			// Assert
			f.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
		}
	}

	public class With_Ok
	{
		[Fact]
		public async Task Returns_Ok_With_Mapped_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Wrap(Rnd.Str);
			var f = Substitute.For<Func<string, int>>();
			f.Invoke(Arg.Any<string>()).Returns(value);

			// Act
			var r0 = await input.MapAsync(async x => f(x));
			var r1 = await input.AsTask().MapAsync(f);
			var r2 = await input.AsTask().MapAsync(async x => f(x));

			// Assert
			r0.AssertOk(value);
			r1.AssertOk(value);
			r2.AssertOk(value);
		}

		[Fact]
		public async Task f_Throws__Returns_Failure()
		{
			// Arrange
			var input = R.Wrap(Rnd.Str);
			var ex = new Exception(Rnd.Str);
			var mapThrow = Substitute.For<Func<string, int>>();
			mapThrow.Invoke(Arg.Any<string>()).ThrowsForAnyArgs(ex);
			var mapThrowAsync = Substitute.For<Func<string, Task<int>>>();
			mapThrowAsync.Invoke(Arg.Any<string>()).ThrowsAsyncForAnyArgs(ex);

			// Act
			var r0 = await input.MapAsync<string, int>(_ => throw ex);
			var r1 = await input.AsTask().MapAsync(mapThrow);
			var r2 = await input.AsTask().MapAsync(mapThrowAsync);

			// Assert
			r0.AssertFailure(ex);
			r1.AssertFailure(ex);
			r2.AssertFailure(ex);
		}

		[Fact]
		public async Task Custom_Handler_Is_Called_On_Exception()
		{
			// Arrange
			var input = R.Wrap(Rnd.Str);
			var handlerCalled = false;
			R.ExceptionHandler handler = _ => { handlerCalled = true; return FailGen.Create(); };

			// Act
			Func<string, int> throwSync = _ => throw new Exception(Rnd.Str);
			_ = await input.MapAsync<string, int>(_ => throw new Exception(Rnd.Str), handler);
			_ = await input.AsTask().MapAsync(throwSync, handler);

			// Assert
			Assert.True(handlerCalled);
		}

		[Fact]
		public async Task Custom_Handler_Result_Is_Returned()
		{
			// Arrange
			var customMessage = Rnd.Str;
			var input = R.Wrap(Rnd.Str);
			R.ExceptionHandler handler = _ => FailGen.Create(new(customMessage));

			// Act
			Func<string, int> throwSync = _ => throw new Exception(Rnd.Str);
			var r0 = await input.MapAsync<string, int>(_ => throw new Exception(Rnd.Str), handler);
			var r1 = await input.AsTask().MapAsync(throwSync, handler);
			var r2 = await input.AsTask().MapAsync(_ => Task.FromException<int>(new Exception(Rnd.Str)), handler);

			// Assert
			r0.AssertFailure(customMessage);
			r1.AssertFailure(customMessage);
			r2.AssertFailure(customMessage);
		}
	}
}
