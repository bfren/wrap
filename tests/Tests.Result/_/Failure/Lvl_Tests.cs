// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Logging;

namespace Wrap.Failure_Tests;

public class Lvl_Tests
{
	public class With_Level
	{
		[Fact]
		public void Adds_Level_To_FailureValue()
		{
			// Arrange
			var failure = FailGen.Create().Lvl(LogLevel.Verbose);
			var value = LogLevel.Fatal;

			// Act
			var result = failure.Lvl(value);

			// Assert
			Assert.Equal(value, result.Value.Level);
		}
	}
}
