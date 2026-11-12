// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Either_Tests;

public class Unwrap_Tests
{
	[Fact]
	public void Left__Returns_GetValue_Result()
	{
		// Arrange
		var value = Rnd.Str;
		var expected = Rnd.Int;
		var getValue = Substitute.For<Func<string, int>>();
		_ = getValue.Invoke(value).Returns(expected);
		Either<string, int> either = new Left<string>(value);

		// Act
		var result = either.Unwrap(getValue);

		// Assert
		Assert.Equal(expected, result);
		_ = getValue.Received(1).Invoke(value);
	}

	[Fact]
	public void Right__Returns_Value()
	{
		// Arrange
		var value = Rnd.Int;
		var getValue = Substitute.For<Func<string, int>>();
		Either<string, int> either = new Right<int>(value);

		// Act
		var result = either.Unwrap(getValue);

		// Assert
		Assert.Equal(value, result);
		_ = getValue.DidNotReceiveWithAnyArgs().Invoke(default!);
	}
}
