// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Result_Tests.FailureImpl_Tests;

public class Equals_Tests
{
	[Fact]
	public void Same_Value_Different_Loggers__Are_Equal()
	{
		// Arrange
		var value = FailGen.Value;
		var withLogger = new Result<int>.FailureImpl(null, Substitute.For<F.FailureLogger>(), value);
		var withoutLogger = new Result<int>.FailureImpl(null, null, value);

		// Act
		var r0 = withLogger.Equals(withoutLogger);
		var r1 = (Result<int>)withLogger == withoutLogger;
		var r2 = withLogger.GetHashCode() == withoutLogger.GetHashCode();

		// Assert
		Assert.True(r0);
		Assert.True(r1);
		Assert.True(r2);
	}
}
