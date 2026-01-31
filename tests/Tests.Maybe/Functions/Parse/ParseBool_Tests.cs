// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Functions_Tests;

public class ParseBool_Tests : Abstracts.Parse_Tests<bool>
{
	[Theory]
	[InlineData("false")]
	[InlineData("true")]
	[InlineData("False")]
	[InlineData("True")]
	[InlineData("FALSE")]
	[InlineData("TRUE")]
	[InlineData("fAlSe")]
	[InlineData("tRuE")]
	public override void Test00_Valid_Input_Returns_Parsed_Result(string? input) =>
		Test00(input, bool.Parse, M.ParseBool, M.ParseBool);

	[Theory]
	[InlineData("")]
	[InlineData("0")]
	[InlineData("1")]
	[InlineData("no")]
	[InlineData("yes")]
	[InlineData("Invalid")]
	public override void Test01_Invalid_Input_Returns_None(string? input) =>
		Test01(input, M.ParseBool, M.ParseBool);

	[Theory]
	[InlineData(null)]
	public override void Test02_Null_Input_Returns_None(string? input) =>
		Test02(input, M.ParseBool, M.ParseBool);
}
