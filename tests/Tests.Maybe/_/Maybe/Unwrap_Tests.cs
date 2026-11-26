// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Maybe_Tests;

public class Unwrap_Tests
{
	public class When_None
	{
		[Fact]
		public void Returns_Value_From_GetValue()
		{
			// Arrange
			var failure = default(Maybe<Guid>);
			var value = Rnd.Guid;
			var getValue = Substitute.For<Func<Guid>>();
			getValue.Invoke().Returns(value);

			// Act
			var result = failure.Unwrap(getValue);

			// Assert
			Assert.Equal(value, result);
		}
	}

	public class When_Some
	{
		[Fact]
		public void Returns_Value()
		{
			// Arrange
			var expected = Rnd.Str;
			var value = M.Wrap(expected);

			// Act
			var result = value.Unwrap(Substitute.For<Func<string>>());

			// Assert
			Assert.Equal(expected, result);
		}
	}
}
