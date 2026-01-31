// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Functions_Tests;

public class Wrap_Tests
{
	public class With_Null
	{
		public class Reference_Type
		{
			[Fact]
			public void Returns_None()
			{
				// Arrange

				// Act
				var result = M.Wrap<string>(null!);

				// Assert
				result.AssertNone();
			}
		}

		public class Nullable_Reference_Type
		{
			[Fact]
			public void Returns_None()
			{
				// Arrange

				// Act
				var result = M.Wrap<string?>(null);

				// Assert
				result.AssertNone();
			}
		}

		public class Nullable_Value_Type
		{
			[Fact]
			public void Returns_None()
			{
				// Arrange

				// Act
				var result = M.Wrap<int?>(null);

				// Assert
				result.AssertNone();
			}
		}
	}

	public class With_Value
	{
		[Fact]
		public void Returns_Some_With_Value()
		{
			// Arrange
			var value = Rnd.UIntPtr;

			// Act
			var result = M.Wrap(value);

			// Assert
			result.AssertSome(value);
		}
	}
}
