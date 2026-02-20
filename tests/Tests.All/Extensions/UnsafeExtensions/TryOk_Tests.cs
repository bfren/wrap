// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.UnsafeExtensions_Tests;

public class TryOk_Tests
{
	public class Ok
	{
		[Fact]
		public void Out_Var_Is_Value()
		{
			// Arrange
			var value = Rnd.Guid;
			var wrapped = R.Wrap(value).Unsafe();

			// Act
			_ = wrapped.TryOk(out var result);

			// Assert
			Assert.Equal(value, result);
		}

		[Fact]
		public void Returns_True()
		{
			// Arrange
			var value = Rnd.Guid;
			var wrapped = R.Wrap(value).Unsafe();

			// Act
			var result = wrapped.TryOk(out var _);

			// Assert
			Assert.True(result);
		}
	}

	public class Failure
	{
		[Fact]
		public void Out_Var_Is_Default()
		{
			// Arrange
			var failure = FailGen.Create<int>().Unsafe();

			// Act
			_ = failure.TryOk(out var result);

			// Assert
			Assert.Equal(default, result);
		}

		[Fact]
		public void Returns_False()
		{
			// Arrange
			var failure = FailGen.Create<int>().Unsafe();

			// Act
			var result = failure.TryOk(out var _);

			// Assert
			Assert.False(result);
		}
	}
}
