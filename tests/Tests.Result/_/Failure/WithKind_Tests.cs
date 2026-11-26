// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Failure_Tests;

public class WithKind_Tests
{
	[Fact]
	public void Sets_Kind_And_Leaves_Original_Unchanged()
	{
		// Arrange
		var failure = R.Fail(Rnd.Str);

		// Act
		var result = failure.withKind(FailureKind.NotFound);

		// Assert
		Assert.Equal(FailureKind.NotFound, result.Kind);
		Assert.Equal(FailureKind.Unspecified, failure.Kind);
		Assert.Equal(failure.Message, result.Message);
	}

	[Fact]
	public void Custom_Kind_Equals_Kind_With_Same_Name()
	{
		// Arrange
		var name = Rnd.Str;

		// Act
		var result = R.Fail(Rnd.Str).withKind(new(name));

		// Assert
		Assert.Equal(new FailureKind(name), result.Kind);
	}
}
