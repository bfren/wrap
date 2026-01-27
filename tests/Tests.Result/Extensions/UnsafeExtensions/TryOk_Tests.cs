// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.UnsafeExtensions_Tests;

public class TryOk_Tests
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
			_ = wrapped.Unsafe().TryOk(out var result);

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
			var result = wrapped.Unsafe().TryOk(out var _);

			// Assert
			Assert.True(result);
		}
	}

	public class input_is_failure
	{
		[Fact]
		public void out_var_is_default()
		{
			// Arrange
			var failure = FailGen.Create<int>();

			// Act
			_ = failure.Unsafe().TryOk(out var result);

			// Assert
			Assert.Equal(default, result);
		}

		[Fact]
		public void returns_false()
		{
			// Arrange
			var failure = FailGen.Create<int>();

			// Act
			var result = failure.Unsafe().TryOk(out var _);

			// Assert
			Assert.False(result);
		}
	}
}
