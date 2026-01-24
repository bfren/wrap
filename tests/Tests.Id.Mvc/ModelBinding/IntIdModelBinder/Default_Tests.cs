// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Mvc.ModelBinding.IntIdModelBinder_Tests;

public class Default_Tests
{
	[Fact]
	public void Returns_Zero()
	{
		// Arrange
		var binder = new IntIdModelBinder<TestIntId>();

		// Act
		var result = binder.Default;

		// Assert
		Assert.Equal(0, Assert.IsType<int>(result));
	}

	public sealed record class TestIntId : IntId<TestIntId>;
}
