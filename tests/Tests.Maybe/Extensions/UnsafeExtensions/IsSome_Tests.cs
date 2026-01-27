// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.UnsafeExtensions_Tests;

public class IsSome_Tests
{
	public class input_is_some
	{
		[Fact]
		public void out_var_is_value()
		{
			// Arrange
			var value = Rnd.Guid;
			var maybe = M.Wrap(value);

			// Act
			_ = maybe.Unsafe().IsSome(out var result);

			// Assert
			Assert.Equal(value, result);
		}

		[Fact]
		public void returns_true()
		{
			// Arrange
			var value = Rnd.Guid;
			var maybe = M.Wrap(value);

			// Act
			var result = maybe.Unsafe().IsSome(out var _);

			// Assert
			Assert.True(result);
		}
	}

	public class input_is_none
	{
		[Fact]
		public void out_var_is_default()
		{
			// Arrange
			var maybe = (Maybe<int>)M.None;

			// Act
			_ = maybe.Unsafe().IsSome(out var result);

			// Assert
			Assert.Equal(default, result);
		}

		[Fact]
		public void returns_false()
		{
			// Arrange
			var maybe = (Maybe<int>)M.None;

			// Act
			var result = maybe.Unsafe().IsSome(out var _);

			// Assert
			Assert.False(result);
		}
	}
}
