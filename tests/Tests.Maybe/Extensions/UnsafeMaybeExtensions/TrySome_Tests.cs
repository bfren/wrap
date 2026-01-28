// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.UnsafeMaybeExtensions_Tests;

public class TrySome_Tests
{
	public class Some
	{
		[Fact]
		public void Out_Var_Is_Value()
		{
			// Arrange
			var value = Rnd.Guid;
			var maybe = M.Wrap(value);

			// Act
			_ = maybe.Unsafe().TrySome(out var result);

			// Assert
			Assert.Equal(value, result);
		}

		[Fact]
		public void Returns_True()
		{
			// Arrange
			var value = Rnd.Guid;
			var maybe = M.Wrap(value);

			// Act
			var result = maybe.Unsafe().TrySome(out var _);

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
			var maybe = (Maybe<int>)M.None;

			// Act
			_ = maybe.Unsafe().TrySome(out var result);

			// Assert
			Assert.Equal(default, result);
		}

		[Fact]
		public void Returns_False()
		{
			// Arrange
			var maybe = (Maybe<int>)M.None;

			// Act
			var result = maybe.Unsafe().TrySome(out var _);

			// Assert
			Assert.False(result);
		}
	}
}
