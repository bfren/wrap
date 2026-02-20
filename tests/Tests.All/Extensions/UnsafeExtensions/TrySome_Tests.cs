// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.UnsafeExtensions_Tests;

public class TrySome_Tests
{
	public class Some
	{
		[Fact]
		public void Out_Var_Is_Value()
		{
			// Arrange
			var value = Rnd.Guid;
			var maybe = M.Wrap(value).Unsafe();

			// Act
			_ = maybe.TrySome(out var result);

			// Assert
			Assert.Equal(value, result);
		}

		[Fact]
		public void Returns_True()
		{
			// Arrange
			var value = Rnd.Guid;
			var maybe = M.Wrap(value).Unsafe();

			// Act
			var result = maybe.TrySome(out var _);

			// Assert
			Assert.True(result);
		}
	}

	public class None
	{
		[Fact]
		public void Our_Var_Is_Default()
		{
			// Arrange
			var maybe = ((Maybe<int>)M.None).Unsafe();

			// Act
			_ = maybe.TrySome(out var result);

			// Assert
			Assert.Equal(default, result);
		}

		[Fact]
		public void Returns_False()
		{
			// Arrange
			var maybe = ((Maybe<int>)M.None).Unsafe();

			// Act
			var result = maybe.TrySome(out var _);

			// Assert
			Assert.False(result);
		}
	}
}
