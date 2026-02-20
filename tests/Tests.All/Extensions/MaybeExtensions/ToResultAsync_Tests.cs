// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Exceptions;

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class ToResultAsync_Tests
{
	public class Invalid
	{
		public class Null_Maybe
		{
			[Fact]
			public async Task Returns_Failure_With_NullReferenceException()
			{
				// Arrange
				Task<Maybe<int>> maybe = null!;

				// Act
				var result = await MaybeExtensions.ToResultAsync(maybe, Rnd.Str, Rnd.Str);

				// Assert
				var f = result.AssertFailure("Error converting Maybe<{Type}> to Result.", nameof(Int32));
				Assert.IsType<NullReferenceException>(f.Exception);
			}
		}

		public class Unknown_Maybe
		{
			[Fact]
			public async Task Returns_Failure_With_InvalidMaybeTypeException()
			{
				// Arrange
				var maybe = new InvalidMaybe<Guid>().AsTask();

				// Act
				var result = await maybe.ToResultAsync(Rnd.Str, Rnd.Str);

				// Assert
				var f = result.AssertFailure("Error converting Maybe<{Type}> to Result.", nameof(Guid));
				Assert.IsType<InvalidMaybeTypeException>(f.Exception);
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
			public async Task Catches_Exception_And_Returns_Failure()
			{
				// Arrange
				var maybe = M.NoneAsTask<string>();
				var handler = Substitute.For<Func<Result<string>>>();
				var ex = new Exception("Test exception");
				handler.Invoke().Returns(x => throw ex);

				// Act
				var r0 = await maybe.ToResultAsync(handler);
				var r1 = await maybe.ToResultAsync(async () => handler());

				// Assert
				var f0 = r0.AssertFailure("Error converting Maybe<{Type}> to Result.", nameof(String));
				Assert.Same(ex, f0.Exception);
				var f1 = r1.AssertFailure("Error converting Maybe<{Type}> to Result.", nameof(String));
				Assert.Same(ex, f1.Exception);
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
