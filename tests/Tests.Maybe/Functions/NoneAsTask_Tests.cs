// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Functions_Tests;

public class NoneAsTask_Tests
{
	[Fact]
	public async Task Returns_None_Wrapped_In_Task()
	{
		// Arrange

		// Act
		var result = await M.NoneAsTask<string>();

		// Assert
		result.AssertNone();
	}
}
