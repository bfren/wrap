// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Failure_Tests;

public class GetHashCode_Tests
{
	[Fact]
	public void Returns_HashCode_Of_Value()
	{
		// Arrange
		var value = new FailureValue(Rnd.Str);
		var failure = new Failure(value);

		// Act
		var result = failure.GetHashCode();

		// Assert
		Assert.Equal(value.GetHashCode(), result);
	}
}
