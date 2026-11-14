// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Exceptions;

namespace Wrap.Extensions.UnsafeExtensions_Tests;

public class Unwrap_Tests
{
	public class With_Left
	{
		[Fact]
		public void Throws_UnsafeUnwrapException_With_Formatted_Message()
		{
			// Arrange
			var arg = Rnd.Int;
			var failure = R.Fail<int>("Value {A} failed", arg).Unsafe();

			// Act
			var result = Record.Exception(() => failure.Unwrap());

			// Assert
			var ex = Assert.IsType<UnsafeUnwrapException>(result);
			Assert.Contains($"Value {arg} failed", ex.Message);
			Assert.Null(ex.InnerException);
		}

		[Fact]
		public void With_Exception__Throws_UnsafeUnwrapException_With_Inner_Exception()
		{
			// Arrange
			var exception = new ArgumentException(Rnd.Str);
			var failure = R.Fail<int>(exception).Unsafe();

			// Act
			var result = Record.Exception(() => failure.Unwrap());

			// Assert
			var ex = Assert.IsType<UnsafeUnwrapException>(result);
			Assert.Same(exception, ex.InnerException);
		}
	}

	public class With_Right
	{
		[Fact]
		public void Returns_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var wrapped = R.Wrap(value).Unsafe();

			// Act
			var result = wrapped.Unwrap();

			// Assert
			Assert.Equal(value, result);
		}
	}

	public class With_None
	{
		[Fact]
		public void Throws_UnsafeUnwrapException()
		{
			// Arrange
			var none = M.Wrap<int?>(null).Unsafe();

			// Act
			var result = Record.Exception(() => none.Unwrap());

			// Assert
			var ex = Assert.IsType<UnsafeUnwrapException>(result);
			Assert.Null(ex.InnerException);
		}
	}

	public class With_Some
	{
		[Fact]
		public void Returns_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var some = M.Wrap(value).Unsafe();

			// Act
			var result = some.Unwrap();

			// Assert
			Assert.Equal(value, result);
		}
	}
}
