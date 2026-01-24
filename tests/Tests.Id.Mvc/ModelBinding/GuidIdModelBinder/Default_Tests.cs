// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Mvc.ModelBinding.GuidIdModelBinder_Tests;

public class Default_Tests
{
	[Fact]
	public void Returns_Guid_Empty()
	{
		// Arrange
		var binder = new GuidIdModelBinder<TestGuidId>();

		// Act
		var result = binder.Default;

		// Assert
		Assert.Equal(Guid.Empty, result);
	}

	public sealed record class TestGuidId : GuidId<TestGuidId>;
}
