// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Abstracts;

public abstract class Parse_Tests<T>
{
	public delegate Maybe<T> ParseSpan(ReadOnlySpan<char> input);

	public delegate Maybe<T> ParseString(string? input);

	public abstract void Test00_Valid_Input_Returns_Parsed_Result(string? input);

	protected static void Test00(string? input, Func<string, T> parse, ParseSpan parseSpan, ParseString parseString)
	{
		// Arrange
		var expected = parse(input ?? string.Empty);

		// Act
		var r0 = parseSpan(input.AsSpan());
		var r1 = parseString(input);

		// Assert
		r0.AssertSome(expected);
		r1.AssertSome(expected);
	}

	public abstract void Test01_Invalid_Input_Returns_None(string? input);

	protected static void Test01(string? input, ParseSpan parseSpan, ParseString parseString)
	{
		// Arrange

		// Act
		var r0 = parseSpan(input.AsSpan());
		var r1 = parseString(input);

		// Assert
		r0.AssertNone();
		r1.AssertNone();
	}

	public abstract void Test02_Null_Input_Returns_None(string? input);

	protected static void Test02(string? input, ParseSpan parseSpan, ParseString parseString)
	{
		// Arrange

		// Act
		var r0 = parseSpan(input.AsSpan());
		var r1 = parseString(input);

		// Assert
		r0.AssertNone();
		r1.AssertNone();
	}
}
