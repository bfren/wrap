// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Some_Tests;

public class Constructor_Tests
{
	public class With_Null
	{
		[Fact]
		public void Reference_Type__Converts_To_None()
		{
			// Arrange
			var some = new Some<string>(null!);

			// Act
			Maybe<string> result = some;

			// Assert
			result.AssertNone();
		}

		[Fact]
		public void Nullable_Reference_Type__Converts_To_None()
		{
			// Arrange
			var some = new Some<string?>(null);

			// Act
			Maybe<string?> result = some;

			// Assert
			result.AssertNone();
		}

		[Fact]
		public void Nullable_Value_Type__Converts_To_None()
		{
			// Arrange
			var some = new Some<int?>(null);

			// Act
			Maybe<int?> result = some;

			// Assert
			result.AssertNone();
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
			var result = new Some<nuint>(value);

			// Assert
			Assert.Equal(value, result.Value);
		}

		[Fact]
		public void Converts_To_Some()
		{
			// Arrange
			var value = Rnd.UIntPtr;
			var some = new Some<nuint>(value);

			// Act
			Maybe<nuint> result = some;

			// Assert
			result.AssertSome(value);
		}
	}
}
