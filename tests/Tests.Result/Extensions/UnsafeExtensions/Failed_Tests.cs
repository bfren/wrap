// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.UnsafeExtensions_Tests;

public class Failed_Tests
{
	public class input_is_ok
	{
		[Fact]
		public void out_var_is_default()
		{
			// Arrange
			var wrapped = R.Wrap(Rnd.Guid);

			// Act
			_ = wrapped.Unsafe().Failed(out var result);

			// Assert
			Assert.Equal(default, result);
		}

		[Fact]
		public void returns_false()
		{
			// Arrange
			var value = Rnd.Guid;
			var wrapped = R.Wrap(value);

			// Act
			var result = wrapped.Unsafe().Failed(out var _);

			// Assert
			Assert.False(result);
		}
	}

	public class input_is_fail
	{
		[Fact]
		public void out_var_is_failure()
		{
			// Arrange
			var failure = FailureValue.Create(Rnd.Str);
			var wrapped = FailGen.Create<int>(failure);

			// Act
			_ = wrapped.Unsafe().Failed(out var result);

			// Assert
			Assert.Equal(failure, result);
		}

		[Fact]
		public void returns_false()
		{
			// Arrange
			var wrapped = FailGen.Create<int>();

			// Act
			var result = wrapped.Unsafe().Failed(out var _);

			// Assert
			Assert.True(result);
		}
	}
}
