// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Functions_Tests;

public class Parse_Tests
{
	public class TryParse_Is_True
	{
		[Fact]
		public void Returns_Value()
		{
			// Arrange
			var valueInt = Rnd.Int;
			var valueSpan = Rnd.Str.AsSpan();
			var valueString = Rnd.Str;

			var tryParseSpan = new M.TryParseSpan<int>((_, out result) =>
			{
				result = valueInt;
				return true;
			});
			var tryParseString = new M.TryParseString<int>((_, out result) =>
			{
				result = valueInt;
				return true;
			});

			// Act
			var r0 = M.Parse(valueSpan, tryParseSpan);
			var r1 = M.Parse(valueString, tryParseString);

			// Assert
			r0.AssertSome(valueInt);
			r1.AssertSome(valueInt);
		}
	}

	public class TryParse_Is_False
	{
		[Fact]
		public void Returns_None()
		{
			// Arrange
			var valueSpan = Rnd.Str.AsSpan();
			var valueString = Rnd.Str;

			var tryParseSpan = new M.TryParseSpan<int>((_, out result) =>
			{
				result = Rnd.Int;
				return false;
			});
			var tryParseString = new M.TryParseString<int>((_, out result) =>
			{
				result = Rnd.Int;
				return false;
			});

			// Act
			var r0 = M.Parse(valueSpan, tryParseSpan);
			var r1 = M.Parse(valueString, tryParseString);

			// Assert
			r0.AssertNone();
			r1.AssertNone();
		}
	}
}
