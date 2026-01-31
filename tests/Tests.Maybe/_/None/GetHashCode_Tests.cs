// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.None_Tests;

public class GetHashCode_Tests
{
	[Fact]
	public void Returns_Zero()
	{
		// Arrange
		var none = new None();

		// Act
		var result = none.GetHashCode();

		// Assert
		Assert.Equal(0, result);
	}
}
