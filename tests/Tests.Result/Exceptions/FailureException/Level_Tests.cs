// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Logging;

namespace Wrap.Exceptions.FailureException_Tests;

public class Level_Tests
{
	[Theory]
	[InlineData(LogLevel.Unknown)]
	[InlineData(LogLevel.Verbose)]
	[InlineData(LogLevel.Debug)]
	[InlineData(LogLevel.Information)]
	[InlineData(LogLevel.Warning)]
	[InlineData(LogLevel.Error)]
	[InlineData(LogLevel.Fatal)]
	public void Returns_Failure_Level(LogLevel input)
	{
		// Arrange
		var value = FailGen.Value with { Level = input };

		// Act
		var result = new FailureException(value);

		// Assert
		Assert.Equal(value.Level, result.Level);
	}
}
