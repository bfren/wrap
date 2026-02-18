// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Ids;

namespace Wrap.Extensions.UInt32Extensions_Tests;

public class WrapId_Tests
{
	[Fact]
	public void Returns_Id_With_Value()
	{
		// Arrange
		var value = Rnd.UInt32;

		// Act
		var result = value.WrapId<TestId>();

		// Assert
		Assert.Equal(value, result.Value);
	}

	public sealed record class TestId : UIntId<TestId>;
}
