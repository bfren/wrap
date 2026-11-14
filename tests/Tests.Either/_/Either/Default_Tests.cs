// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Either_Tests;

public class Default_Tests
{
	[Fact]
	public void Is_Left_With_Default_Value()
	{
		// Arrange
		var either = default(Either<string, int>);

		// Act
		var result = either.Value;

		// Assert
		var left = Assert.IsType<Left<string>>(result);
		Assert.Null(left.Value);
	}

	[Fact]
	public void Matches_Left_Pattern()
	{
		// Arrange
		var either = default(Either<int, string>);

		// Act
		var result = either switch
		{
			Left<int> l =>
				l.Value,

			Right<string> =>
				-1
		};

		// Assert
		Assert.Equal(0, result);
	}
}
