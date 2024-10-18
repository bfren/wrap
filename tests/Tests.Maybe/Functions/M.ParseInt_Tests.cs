// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.M_Tests;

public abstract class ParseInt_Tests<T>
{
	protected abstract T GetRndValue();

	protected delegate Maybe<T> ParseString(string? input);

	protected delegate Maybe<T> ParseSpan(ReadOnlySpan<char> input);

	[Fact]
	public abstract void return_some_with_int_value__when_value_is_parseable_int();

	protected void Test00(ParseString parseString, ParseSpan parseSpan)
	{
		// Arrange
		var value = GetRndValue();
		var valueStr = value?.ToString();

		// Act
		var r0 = parseString(valueStr);
		var r1 = parseSpan(valueStr.AsSpan());

		// Assert
		var s0 = r0.AssertSome();
		Assert.Equal(value, s0);
		var s1 = r1.AssertSome();
		Assert.Equal(value, s1);
	}

	[Fact]
	public abstract void return_none__when_value_is_null();

	protected void Test01(ParseString parseString, ParseSpan parseSpan)
	{
		// Arrange
		string? value = null;

		// Act
		var r0 = parseString(value);
		var r1 = parseSpan(value.AsSpan());

		// Assert
		r0.AssertNone();
		r1.AssertNone();
	}

	[Theory]
	[InlineData(null)]
	[InlineData("Invalid")]
	public abstract void return_none__when_value_is_not_parseable_int(string? input);

	protected void Test02(string? input, ParseString parseString, ParseSpan parseSpan)
	{
		// Arrange

		// Act
		var r0 = parseString(input);
		var r1 = parseSpan(input.AsSpan());

		// Assert
		r0.AssertNone();
		r1.AssertNone();
	}
}
