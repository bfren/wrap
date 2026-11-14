// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Failure_Tests;

public class GetHashCode_Tests
{
	[Fact]
	public void Equal_Failures_With_Separate_Args_Arrays_Have_Same_HashCode()
	{
		// Arrange
		var message = Rnd.Str;
		var (a0, a1) = (Rnd.Int, Rnd.Str);
		var f0 = new Failure(message, a0, a1);
		var f1 = new Failure(message, a0, a1);

		// Act
		var r0 = f0.GetHashCode();
		var r1 = f1.GetHashCode();

		// Assert
		Assert.Equal(r0, r1);
	}

	[Fact]
	public void Default_Does_Not_Throw()
	{
		// Arrange

		// Act
		var result = Record.Exception(() => default(Failure).GetHashCode());

		// Assert
		Assert.Null(result);
	}
}
