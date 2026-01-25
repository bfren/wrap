// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.UnsafeExtensions_Tests;

public class IsOk_Tests
{
	public class input_is_ok
	{
		[Fact]
		public void out_var_is_value()
		{
			// Arrange
			var value = Rnd.Guid;
			var wrapped = R.Wrap(value);

			// Act
			_ = wrapped.Unsafe().IsOk(out var result);

			// Assert
			Assert.Equal(value, result);
		}

		[Fact]
		public void returns_true()
		{
			// Arrange
			var value = Rnd.Guid;
			var wrapped = R.Wrap(value);

			// Act
			var result = wrapped.Unsafe().IsOk(out var _);

			// Assert
			Assert.True(result);
		}
	}

	public class input_is_fail
	{
		[Fact]
		public void out_var_is_default()
		{
			// Arrange
			var wrapped = FailGen.Create<int>();

			// Act
			_ = wrapped.Unsafe().IsOk(out var result);

			// Assert
			Assert.Equal(default, result);
		}

		[Fact]
		public void returns_false()
		{
			// Arrange
			var wrapped = FailGen.Create<int>();

			// Act
			var result = wrapped.Unsafe().IsOk(out var _);

			// Assert
			Assert.False(result);
		}
	}
}
