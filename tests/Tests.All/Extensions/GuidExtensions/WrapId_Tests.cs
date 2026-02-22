// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Ids;

namespace Wrap.Extensions.GuidExtensions_Tests;

public class WrapId_Tests
{
	[Fact]
	public void Returns_Id_With_Value()
	{
		// Arrange
		var value = Rnd.Guid;

		// Act
		var result = value.WrapId<TestId>();

		// Assert
		Assert.Equal(value, result.Value);
	}

	public sealed record class TestId : GuidId<TestId>;
}
