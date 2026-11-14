// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Either_Tests;

public class ToString_Tests
{
	[Fact]
	public void Left__Returns_Left_String()
	{
		// Arrange
		var value = Rnd.Str;
		Either<string, int> either = new Left<string>(value);

		// Act
		var result = either.ToString();

		// Assert
		Assert.Equal($"Left {{ Value = {value} }}", result);
	}

	[Fact]
	public void Right__Returns_Right_String()
	{
		// Arrange
		var value = Rnd.Int;
		Either<string, int> either = new Right<int>(value);

		// Act
		var result = either.ToString();

		// Assert
		Assert.Equal($"Right {{ Value = {value} }}", result);
	}
}
