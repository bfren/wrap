// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Maybe_Tests;

public class GetEnumerator_Tests
{
	public class When_None
	{
		[Fact]
		public void Does_Not_Enumerate()
		{
			// Arrange
			var value = NoneGen.Create<int>();

			// Act
			var result = 0;
			var counter = 0;
			foreach (var item in value)
			{
				result = item;
				counter++;
			}

			// Assert
			Assert.Equal(0, result);
			Assert.Equal(0, counter);
		}
	}

	public class When_Some
	{
		[Fact]
		public void Enumerates_And_Returns_Value()
		{
			// Arrange
			var expected = Rnd.Int;
			var value = M.Wrap(expected);

			// Act
			var result = 0;
			var counter = 0;
			foreach (var item in value)
			{
				result = item;
				counter++;
			}

			// Assert
			Assert.Equal(expected, result);
			Assert.Equal(1, counter);
		}
	}
}
