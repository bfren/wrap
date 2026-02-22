// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.DictionaryExtensions_Tests;

public class GetValueOrNone_Tests
{
	public class With_Key_Not_Found
	{
		[Fact]
		public void Returns_None()
		{
			// Arrange
			var dict = new Dictionary<string, int> { [Rnd.Str] = Rnd.Int };
			var key = Rnd.Str;

			// Act
			var result = dict.GetValueOrNone(key);

			// Assert
			result.AssertNone();
		}
	}

	public class With_Key_Found
	{
		[Fact]
		public void Returns_Some_With_Value()
		{
			// Arrange
			var key = Rnd.Str;
			var value = Rnd.Int;
			var dict = new Dictionary<string, int> { [key] = value };

			// Act
			var result = dict.GetValueOrNone(key);

			// Assert
			result.AssertSome(value);
		}
	}
}
