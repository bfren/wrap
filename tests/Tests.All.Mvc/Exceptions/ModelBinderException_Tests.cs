// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Exceptions;

namespace Wrap.Mvc.Exceptions_Tests;

public class ModelBinderException_Tests
{
	[Fact]
	public void Is_WrapException()
	{
		// Arrange

		// Act
		var result = new ModelBinderException();

		// Assert
		_ = Assert.IsAssignableFrom<WrapException>(result);
	}
}
