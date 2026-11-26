// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Logging;

namespace Wrap.Failure_Tests;

public class WithLevel_Tests
{
	public class With_Level
	{
		[Fact]
		public void Adds_Level_To_Failure()
		{
			// Arrange
			var failure = R.Fail(Rnd.Str).WithLevel(LogLevel.Verbose);
			var value = LogLevel.Fatal;

			// Act
			var result = failure.WithLevel(value);

			// Assert
			Assert.Equal(value, result.Level);
		}
	}
}
