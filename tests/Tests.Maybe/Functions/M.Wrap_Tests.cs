// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.M_Tests;

public class Wrap_Tests
{
	[Fact]
	public void return_some_with_null_value__when_value_is_null_and_type_is_nullable_value_type()
	{
		// Arrange
		nint? value = null;

		// Act
		var result = M.Wrap(value);

		// Assert
		var some = result.AssertSome();
		Assert.Null(some);
	}
	[Fact]
	public void return_none__when_value_is_null_and_type_is_reference_type()
	{
		// Arrange
		string? value = null;

		// Act
		var result = M.Wrap(value);

		// Assert
		result.AssertNone();
	}

	[Fact]
	public void return_some_with_value__when_value_is_not_null()
	{
		// Arrange
		var value = Rnd.Ptr;

		// Act
		var result = M.Wrap(value);

		// Assert
		var some = result.AssertSome();
		Assert.Equal(value, some);
	}
}
