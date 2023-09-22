// Monads: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Monads.FunctionTests.IsNullableTests;

public class when_input_is_nullable_value_type
{
	[Fact]
	public void returns_true()
	{
		// Arrange
		int? value = null;

		// Act
		var result = F.IsNullableValueType(value);

		// Assert
		Assert.True(result);
	}
}

public class when_input_is_non_nullable_value_type
{
	[Fact]
	public void returns_false()
	{
		// Arrange
		var value = 0;

		// Act
		var result = F.IsNullableValueType(value);

		// Assert
		Assert.False(result);
	}
}

public class when_input_is_reference_type
{
	[Fact]
	public void returns_false()
	{
		// Arrange
		var v0 = string.Empty;
		string? v1 = null;

		// Act
		var r0 = F.IsNullableValueType(v0);
		var r1 = F.IsNullableValueType(v1);

		// Assert
		Assert.False(r0);
		Assert.False(r1);
	}
}
