// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Ok_Tests;

public class Constructor_Tests
{
	public class With_Null
	{
		public class Reference_Type
		{
			[Fact]
			public void Throws_ArgumentNullException()
			{
				// Arrange

				// Act
				var result = Record.Exception(() => new Ok<string>(null!));

				// Assert
				Assert.IsType<ArgumentNullException>(result);
			}
		}

		public class Nullable_Reference_Type
		{
			[Fact]
			public void Throws_ArgumentNullException()
			{
				// Arrange

				// Act
				var result = Record.Exception(() => new Ok<string?>(null!));

				// Assert
				Assert.IsType<ArgumentNullException>(result);
			}
		}

		public class Nullable_Value_Type
		{
			[Fact]
			public void Throws_ArgumentNullException()
			{
				// Arrange

				// Act
				var result = Record.Exception(() => new Ok<int?>(null!));

				// Assert
				Assert.IsType<ArgumentNullException>(result);
			}
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
	}
}
