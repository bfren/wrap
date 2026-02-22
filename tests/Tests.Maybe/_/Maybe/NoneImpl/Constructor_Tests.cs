// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Maybe_Tests.NoneImpl_Tests;

public class Constructor_Tests
{
	[Fact]
	public void Sets_Value_To_NoneImpl()
	{
		// Arrange

		// Act
		var result = new Maybe<int>.NoneImpl();

		// Assert
		Assert.Equal(M.None, result.Value);
	}
}
