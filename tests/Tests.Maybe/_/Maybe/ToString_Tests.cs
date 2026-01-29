// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Maybe_Tests;

public class ToString_Tests
{
	public class When_None
	{
		[Fact]
		public void Returns_None_With_Type()
		{
			// Arrange
			var none = NoneGen.Create<DateTime>();
			var expected = $"None: {nameof(DateTime)}";

			// Act
			var result = none.ToString();

			// Assert
			Assert.Equal(expected, result);
		}
	}

	public class When_Some
	{
		public class When_Value_ToString_Returns_Null
		{
			[Fact]
			public void Returns_Ok_With_Type()
			{
				// Arrange
				var wrapped = M.Wrap(new Test());
				var expected = $"Some: {nameof(Test)}";

				// Act
				var result = wrapped.ToString();

				// Assert
				Assert.Equal(expected, result);
			}
		}

		[Fact]
		public void Returns_Some_Value()
		{
			// Arrange
			var value = Rnd.Guid;
			var wrapped = M.Wrap(value);

			// Act
			var result = wrapped.ToString();

			// Assert
			Assert.Equal(value.ToString(), result);
		}
	}

	public class Test
	{
		public override string ToString() =>
			null!;
	}
}
