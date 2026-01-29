// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Exceptions;

namespace Wrap.Result_Tests;

public class Unwrap_Tests
{
	public class Without_Handlers
	{
		public class When_Failure
		{
			[Fact]
			public void Throws_FailureException()
			{
				// Arrange
				var value = FailGen.Create<int>();

				// Act
				var result = Record.Exception(() => value.Unwrap());

				// Assert
				Assert.IsType<FailureException>(result);
			}
		}

		public class When_Ok
		{
			[Fact]
			public void Returns_Value()
			{
				// Arrange
				var expected = Rnd.Int;
				var value = R.Wrap(expected);

				// Act
				var result = value.Unwrap();

				// Assert
				Assert.Equal(expected, result);
			}
		}
	}

	public class With_Handlers
	{
		public class When_Failure
		{
			[Fact]
			public void Calls_Failure_Handler()
			{
				// Arrange
				var value = FailGen.Value;
				var failure = FailGen.Create<int>(value);
				var handler = Substitute.For<Action<FailureValue>>();

				// Act
				var result = failure.Unwrap(handler, Substitute.For<Func<int>>());

				// Assert
				handler.Received(1).Invoke(value);
			}

			[Fact]
			public void Returns_Value_From_GetValue()
			{
				// Arrange
				var failure = FailGen.Create<Guid>();
				var value = Rnd.Guid;
				var getValue = Substitute.For<Func<Guid>>();
				getValue.Invoke().Returns(value);

				// Act
				var result = failure.Unwrap(Substitute.For<Action<FailureValue>>(), getValue);

				// Assert
				Assert.Equal(value, result);
			}
		}

		public class When_Ok
		{
			[Fact]
			public void Returns_Value()
			{
				// Arrange
				var expected = Rnd.Str;
				var value = R.Wrap(expected);

				// Act
				var result = value.Unwrap(Substitute.For<Action<FailureValue>>(), Substitute.For<Func<string>>());

				// Assert
				Assert.Equal(expected, result);
			}
		}
	}
}
