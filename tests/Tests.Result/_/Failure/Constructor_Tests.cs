// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Failure_Tests;

public class Constructor_Tests
{
	public class With_Message_And_Args
	{
		[Fact]
		public void Sets_Message_Args_And_Warning_Level()
		{
			// Arrange
			var message = Rnd.Str;
			var args = new object?[] { Rnd.Int, Rnd.Str, Rnd.Date };

			// Act
			var result = new Failure(message, args);

			// Assert
			Assert.Equal(message, result.Message);
			Assert.Equal(args, result.Args);
			Assert.Equal(Logging.LogLevel.Warning, result.Level);
			Assert.Equal(FailureKind.Unspecified, result.Kind);
		}
	}

	public class With_Exception
	{
		[Fact]
		public void Sets_Exception_Message_And_Error_Level()
		{
			// Arrange
			var ex = new Exception(Rnd.Str);

			// Act
			var result = new Failure(ex);

			// Assert
			Assert.Equal(ex, result.Exception);
			Assert.Equal(ex.Message, result.Message);
			Assert.Equal(Logging.LogLevel.Error, result.Level);
			Assert.Equal(FailureKind.Unexpected, result.Kind);
		}

		[Fact]
		public void Cancellation_Sets_Cancelled_Kind()
		{
			// Arrange
			var ex = new TaskCanceledException();

			// Act
			var result = new Failure(ex);

			// Assert
			Assert.Equal(FailureKind.Cancelled, result.Kind);
		}

		[Fact]
		public void Timeout_Sets_Timeout_Kind()
		{
			// Arrange
			var ex = new TimeoutException();

			// Act
			var result = new Failure(ex);

			// Assert
			Assert.Equal(FailureKind.Timeout, result.Kind);
		}

		[Fact]
		public void Cancellation_Caused_By_Timeout_Sets_Timeout_Kind()
		{
			// Arrange
			var ex = new TaskCanceledException(Rnd.Str, new TimeoutException());

			// Act
			var result = new Failure(ex);

			// Assert
			Assert.Equal(FailureKind.Timeout, result.Kind);
		}

		[Fact]
		public void Blocking_On_Task_That_Timed_Out_Sets_Timeout_Kind()
		{
			// Arrange
			var task = Task.FromException<int>(new TaskCanceledException(Rnd.Str, new TimeoutException()));

			// Act
			var result = R.Try(() => task.Result).AssertFailure();

			// Assert
			Assert.IsType<AggregateException>(result.Exception);
			Assert.Equal(FailureKind.Timeout, result.Kind);
		}

		[Fact]
		public void Aggregate_With_One_Cancellation_Sets_Cancelled_Kind()
		{
			// Arrange
			var ex = new AggregateException(new OperationCanceledException());

			// Act
			var result = new Failure(ex);

			// Assert
			Assert.Equal(FailureKind.Cancelled, result.Kind);
		}

		[Fact]
		public void Nested_Aggregate_With_One_Timeout_Sets_Timeout_Kind()
		{
			// Arrange
			var ex = new AggregateException(new AggregateException(new TimeoutException()));

			// Act
			var result = new Failure(ex);

			// Assert
			Assert.Equal(FailureKind.Timeout, result.Kind);
		}

		[Fact]
		public void Aggregate_With_Several_Exceptions_Sets_Unexpected_Kind()
		{
			// Arrange
			var ex = new AggregateException(new TimeoutException(), new TimeoutException());

			// Act
			var result = new Failure(ex);

			// Assert
			Assert.Equal(FailureKind.Unexpected, result.Kind);
		}

		[Fact]
		public void Aggregate_With_One_Other_Exception_Sets_Unexpected_Kind()
		{
			// Arrange
			var ex = new AggregateException(new InvalidOperationException());

			// Act
			var result = new Failure(ex);

			// Assert
			Assert.Equal(FailureKind.Unexpected, result.Kind);
		}
	}
}
