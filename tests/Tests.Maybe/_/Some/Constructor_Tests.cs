// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Some_Tests;

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
				var result = Record.Exception(() => new Some<string>(null!));

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
				var result = Record.Exception(() => new Some<string?>(null!));

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
				var result = Record.Exception(() => new Some<int?>(null!));

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
			var value = Rnd.UPtr;

			// Act
			var result = new Some<nuint>(value);

			// Assert
			Assert.Equal(value, result.Value);
		}
	}
}
