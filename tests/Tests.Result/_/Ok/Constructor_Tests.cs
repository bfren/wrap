// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Ok_Tests;

public class Constructor_Tests
{
	private const string NullMessage = "Null Result value - try using Maybe<T> to wrap null values safely.";

	public class With_Null
	{
		[Fact]
		public void Reference_Type__Converts_To_Failure()
		{
			// Arrange
			var ok = new Ok<string>(null!);

			// Act
			Result<string> result = ok;

			// Assert
			_ = result.AssertFailure(NullMessage);
		}

		[Fact]
		public void Nullable_Reference_Type__Converts_To_Failure()
		{
			// Arrange
			var ok = new Ok<string?>(null);

			// Act
			Result<string?> result = ok;

			// Assert
			_ = result.AssertFailure(NullMessage);
		}

		[Fact]
		public void Nullable_Value_Type__Converts_To_Failure()
		{
			// Arrange
			var ok = new Ok<int?>(null);

			// Act
			Result<int?> result = ok;

			// Assert
			_ = result.AssertFailure(NullMessage);
		}
	}

	public class With_Value
	{
		[Fact]
		public void Sets_Value_Property()
		{
			// Arrange
			var value = Rnd.UIntPtr;

			// Act
			var result = new Ok<nuint>(value);

			// Assert
			Assert.Equal(value, result.Value);
		}

		[Fact]
		public void Converts_To_Ok()
		{
			// Arrange
			var value = Rnd.UIntPtr;
			var ok = new Ok<nuint>(value);

			// Act
			Result<nuint> result = ok;

			// Assert
			result.AssertOk(value);
		}
	}
}
