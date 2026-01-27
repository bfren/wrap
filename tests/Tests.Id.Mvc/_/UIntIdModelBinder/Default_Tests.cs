// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Ids;

namespace Wrap.Mvc.UIntIdModelBinder_Tests;

public class Default_Tests
{
	[Fact]
	public void Returns_Zero()
	{
		// Arrange
		var binder = new UIntIdModelBinder<TestUIntId>();

		// Act
		var result = binder.Default;

		// Assert
		Assert.Equal(0u, Assert.IsType<uint>(result));
	}

	public sealed record class TestUIntId : UIntId<TestUIntId>;
}
