// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Extensions;

namespace Wrap.Result_Tests;

public class GetEnumerator_Tests
{
	public class When_Failure
	{
		[Fact]
		public void Does_Not_Enumerate()
		{
			// Arrange
			var value = FailGen.Create<int>();

			// Act
			var result = 0;
			var counter = 0;
			foreach (var item in value.Unsafe())
			{
				result = item;
				counter++;
			}

			// Assert
			Assert.Equal(0, result);
			Assert.Equal(0, counter);
		}
	}

	public class When_Ok
	{
		[Fact]
		public void Enumerates_And_Returns_Value()
		{
			// Arrange
			var expected = Rnd.Int;
			var value = R.Wrap(expected);

			// Act
			var result = 0;
			var counter = 0;
			foreach (var item in value.Unsafe())
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
