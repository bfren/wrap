// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Result_Tests;

public class Unwrap_Tests
{
	public class With_Handlers
	{
		public class When_Failure
		{
			[Fact]
			public void Calls_Failure_Handler()
			{
				// Arrange
				var value = FailGen.Create();
				var failure = FailGen.Create<int>(value);
				var handler = Substitute.For<Action<Failure>>();

				// Act
				_ = failure.Unwrap(f => { handler(f); return Rnd.Int; });
				_ = failure.Unwrap(handler, Substitute.For<Func<int>>());

				// Assert
				handler.Received(2).Invoke(value);
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
				var r0 = failure.Unwrap(_ => getValue());
				var r1 = failure.Unwrap(Substitute.For<Action<Failure>>(), getValue);

				// Assert
				Assert.Equal(value, r0);
				Assert.Equal(value, r1);
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
				var result = value.Unwrap(Substitute.For<Action<Failure>>(), Substitute.For<Func<string>>());

				// Assert
				Assert.Equal(expected, result);
			}
		}
	}
}
