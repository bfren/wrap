// Monadic: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Monadic.FunctionTests.IsNullableTests;

public class when_input_is_nullable_value_type
{
	[Fact]
	public void returns_true()
	{
		// Arrange
		int? value = null;

		// Act
		var result = F.IsNullable(value);

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
		var result = F.IsNullable(value);

		// Assert
		Assert.False(result);
	}
}

public class when_input_is_reference_type
{
	[Fact]
	public void returns_true()
	{
		// Arrange
		var v0 = string.Empty;
		string? v1 = null;

		// Act
		var r0 = F.IsNullable(v0);
		var r1 = F.IsNullable(v1);

		// Assert
		Assert.True(r0);
		Assert.True(r1);
	}
}
