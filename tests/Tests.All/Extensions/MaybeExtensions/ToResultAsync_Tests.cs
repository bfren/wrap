// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class ToResultAsync_Tests
{
	public class Invalid
	{
		public class Null_Maybe
		{
			[Fact]
			public async Task Throws_NullReferenceException()
			{
				// Arrange
				Task<Maybe<int>> maybe = null!;

				// Act
				var action = () => MaybeExtensions.ToResultAsync(maybe, Rnd.Str, Rnd.Str);

				// Assert
				_ = await Assert.ThrowsAsync<NullReferenceException>(action);
			}

			[Fact]
			public async Task With_Handler_Throws_NullReferenceException()
			{
				// Arrange
				Task<Maybe<int>> maybe = null!;
				var handler = Substitute.For<Func<Result<int>>>();

				// Act
				var a0 = () => maybe.ToResultAsync(handler);
				var a1 = () => maybe.ToResultAsync(async () => handler());

				// Assert
				_ = await Assert.ThrowsAsync<NullReferenceException>(a0);
				_ = await Assert.ThrowsAsync<NullReferenceException>(a1);
				handler.DidNotReceive().Invoke();
			}
		}
	}

	public class None
	{
		public class With_Class_And_Function
		{
			[Fact]
			public async Task Returns_Failure_With_Correct_Message()
			{
				// Arrange
				var maybe = M.NoneAsTask<int>();

				// Act
				var result = await maybe.ToResultAsync(Rnd.Str, Rnd.Str);

				// Assert
				_ = result.AssertFailure("Maybe<{Type}> was 'None'.", nameof(Int32));
			}

			[Fact]
			public async Task Returns_Failure_With_Correct_Context()
			{
				// Arrange
				var maybe = M.NoneAsTask<long>();
				var className = Rnd.Str;
				var functionName = Rnd.Str;
				var context = string.Format("{0}.{1}()", className, functionName);

				// Act
				var result = await maybe.ToResultAsync(className, functionName);

				// Assert
				var f = result.AssertFailure();
				Assert.Equal(context, f.Context);
			}
		}

		public class With_Handler
		{
			[Fact]
			public async Task Executes_Handler()
			{
				// Arrange
				var maybe = M.NoneAsTask<string>();
				var handler = Substitute.For<Func<Result<string>>>();

				// Act
				_ = await maybe.ToResultAsync(handler);
				_ = await maybe.ToResultAsync(async () => handler());

				// Assert
				handler.Received(2).Invoke();
			}

			[Fact]
			public async Task Does_Not_Catch_Exception()
			{
				// Arrange
				var maybe = M.NoneAsTask<string>();
				var handler = Substitute.For<Func<Result<string>>>();
				var ex = new Exception(Rnd.Str);
				handler.Invoke().Returns(x => throw ex);

				// Act
				var r0 = await Record.ExceptionAsync(() => maybe.ToResultAsync(handler));
				var r1 = await Record.ExceptionAsync(() => maybe.ToResultAsync(async () => handler()));

				// Assert
				Assert.Same(ex, r0);
				Assert.Same(ex, r1);
			}
		}
	}

	public class Some
	{
		public class With_Class_And_Function
		{
			[Fact]
			public async Task Returns_Wrapped_Value()
			{
				// Arrange
				var value = Rnd.Int;
				var maybe = M.Wrap(value).AsTask();

				// Act
				var result = await maybe.ToResultAsync(Rnd.Str, Rnd.Str);

				// Assert
				result.AssertOk(value);
			}
		}

		public class With_Handler
		{
			[Fact]
			public async Task Returns_Wrapped_Value()
			{
				// Arrange
				var value = Rnd.Int;
				var maybe = M.Wrap(value).AsTask();
				var handler = Substitute.For<Func<Result<int>>>();

				// Act
				var r0 = await maybe.ToResultAsync(handler);
				var r1 = await maybe.ToResultAsync(async () => handler());

				// Assert
				r0.AssertOk(value);
				r1.AssertOk(value);
				handler.DidNotReceive().Invoke();
			}
		}
	}
}
