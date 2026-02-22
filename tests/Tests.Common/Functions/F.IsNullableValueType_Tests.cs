// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.F_Tests;

public class IsNullableValueType_Tests
{
	[Fact]
	public void return_true__when_input_is_nullable_value_type()
	{
		// Arrange
		nint? value = null;

		// Act
		var result = F.IsNullableValueType(value);

		// Assert
		Assert.True(result);
	}

	[Fact]
	public void return_false__when_input_is_non_nullable_value_type()
	{
		// Arrange
		var value = 0;

		// Act
		var result = F.IsNullableValueType(value);

		// Assert
		Assert.False(result);
	}

	[Fact]
	public void return_false__when_input_is_reference_type()
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
