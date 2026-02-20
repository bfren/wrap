// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Exceptions;

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class ToResult_Tests
{
	public class Invalid
	{
		public class Null_Maybe
		{
			[Fact]
			public void Returns_Failure_With_NullMaybeException()
			{
				// Arrange
				Maybe<int> maybe = null!;

				// Act
				var result = maybe.ToResult(Rnd.Str, Rnd.Str);

				// Assert
				var f = result.AssertFailure("Error converting Maybe<{Type}> to Result.", nameof(Int32));
				_ = Assert.IsType<NullMaybeException>(f.Exception);
			}
		}

		public class Unknown_Maybe
		{
			[Fact]
			public void Returns_Failure_With_InvalidMaybeTypeException()
			{
				// Arrange
				var maybe = new InvalidMaybe<Guid>();

				// Act
				var result = maybe.ToResult(Rnd.Str, Rnd.Str);

				// Assert
				var f = result.AssertFailure("Error converting Maybe<{Type}> to Result.", nameof(Guid));
				_ = Assert.IsType<InvalidMaybeTypeException>(f.Exception);
			}
		}
	}

	public class None
	{
		public class With_Class_And_Function
		{
			[Fact]
			public void Returns_Failure_With_Correct_Message()
			{
				// Arrange
				Maybe<int> maybe = M.None;

				// Act
				var result = maybe.ToResult(Rnd.Str, Rnd.Str);

				// Assert
				_ = result.AssertFailure("Maybe<{Type}> was 'None'.", nameof(Int32));
			}

			[Fact]
			public void Returns_Failure_With_Correct_Context()
			{
				// Arrange
				Maybe<int> maybe = M.None;
				var className = Rnd.Str;
				var functionName = Rnd.Str;
				var context = string.Format("{0}.{1}()", className, functionName);

				// Act
				var result = maybe.ToResult(className, functionName);

				// Assert
				var f = result.AssertFailure();
				Assert.Equal(context, f.Context);
			}
		}

		public class With_Handler
		{
			[Fact]
			public void Executes_Handler()
			{
				// Arrange
				Maybe<string> maybe = M.None;
				var handler = Substitute.For<Func<Result<string>>>();

				// Act
				_ = maybe.ToResult(handler);

				// Assert
				handler.Received(1).Invoke();
			}

			[Fact]
			public void Catches_Exception_And_Returns_Failure()
			{
				// Arrange
				Maybe<string> maybe = M.None;
				var handler = Substitute.For<Func<Result<string>>>();
				var ex = new Exception(Rnd.Str);
				handler.Invoke().Returns(x => throw ex);

				// Act
				var result = maybe.ToResult(handler);

				// Assert
				var f = result.AssertFailure("Error converting Maybe<{Type}> to Result.", nameof(String));
				Assert.Same(ex, f.Exception);
			}
		}
	}

	public class Some
	{
		[Fact]
		public void Returns_Wrapped_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var maybe = M.Wrap(value);

			// Act
			var result = maybe.ToResult(Rnd.Str, Rnd.Str);

			// Assert
			result.AssertOk(value);
		}
	}
}
