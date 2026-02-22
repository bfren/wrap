// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Functions_Tests;

public class ParseChar_Tests : Abstracts.Parse_Tests<char>
{
	[Theory]
	[InlineData("a")]
	[InlineData("1")]
	public override void Test00_Valid_Input_Returns_Parsed_Result(string? input) =>
		Test00(input, char.Parse, M.ParseChar, M.ParseChar);

	[Theory]
	[InlineData("true")]
	public override void Test01_Invalid_Input_Returns_None(string? input) =>
		Test01(input, M.ParseChar, M.ParseChar);

	[Theory]
	[InlineData(null)]
	public override void Test02_Null_Input_Returns_None(string? input) =>
		Test02(input, M.ParseChar, M.ParseChar);
}
