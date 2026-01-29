// Wrap: Unit Tests.
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
			var failure = NoneGen.Create<Guid>();
			var value = Rnd.Guid;
			var getValue = Substitute.For<Func<Guid>>();
			getValue.Invoke().Returns(value);

			// Act
			var r0 = failure.Unwrap(getValue);
			var r1 = failure.Unwrap(n => getValue());

			// Assert
			Assert.Equal(value, r0);
			Assert.Equal(value, r1);
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
			var r0 = value.Unwrap(Substitute.For<Func<string>>());
			var r1 = value.Unwrap(Substitute.For<Func<None, string>>());

			// Assert
			Assert.Equal(expected, r0);
			Assert.Equal(expected, r1);
		}
	}
}
