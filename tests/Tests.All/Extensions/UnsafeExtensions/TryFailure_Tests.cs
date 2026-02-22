// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.UnsafeExtensions_Tests;

public class TryFailure_Tests
{
	public class Ok
	{
		[Fact]
		public void Our_Var_Is_Default()
		{
			// Arrange
			var wrapped = R.Wrap(Rnd.Guid).Unsafe();

			// Act
			_ = wrapped.TryFailure(out var result);

			// Assert
			Assert.Equal(default, result);
		}

		[Fact]
		public void Returns_False()
		{
			// Arrange
			var value = Rnd.Guid;
			var wrapped = R.Wrap(value).Unsafe();

			// Act
			var result = wrapped.TryFailure(out var _);

			// Assert
			Assert.False(result);
		}
	}

	public class Failure
	{
		[Fact]
		public void Out_Var_Is_Failure()
		{
			// Arrange
			var value = new FailureValue(Rnd.Str);
			var failure = FailGen.Create<int>(value).Unsafe();

			// Act
			_ = failure.TryFailure(out var result);

			// Assert
			Assert.Equal(value, result);
		}

		[Fact]
		public void Returns_False()
		{
			// Arrange
			var failure = FailGen.Create<int>().Unsafe();

			// Act
			var result = failure.TryFailure(out var _);

			// Assert
			Assert.True(result);
		}
	}
}
