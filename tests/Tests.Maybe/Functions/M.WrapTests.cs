// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Functions.WrapTests;

public class when_value_is_null
{
	public class and_type_is_nullable_value_type
	{
		[Fact]
		public void returns_some_with_null_value()
		{
			// Arrange
			int? value = null;

			// Act
			var result = M.Wrap(value);

			// Assert
			var some = result.AssertSome();
			Assert.Null(some);
		}
	}

	public class and_type_is_reference_type
	{
		[Fact]
		public void returns_none()
		{
			// Arrange
			string? value = null;

			// Act
			var result = M.Wrap(value);

			// Assert
			result.AssertNone();
		}
	}
}

public class when_value_is_not_null
{
	[Fact]
	public void returns_some_with_value()
	{
		// Arrange
		var value = Rnd.Int;

		// Act
		var result = M.Wrap(value);

		// Assert
		var some = result.AssertSome();
		Assert.Equal(value, some);
	}
}
