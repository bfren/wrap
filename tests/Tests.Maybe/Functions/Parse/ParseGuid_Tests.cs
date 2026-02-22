// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Functions_Tests;

public class ParseGuid_Tests : Abstracts.Parse_Tests<Guid>
{
	[Theory]
	[InlineData("00000000-0000-0000-0000-000000000000")]
	[InlineData("00000000000000000000000000000000")]
	[InlineData("e402617b-d4fa-4abe-81d7-952695860b51")]
	[InlineData("e402617bd4fa4abe81d7952695860b51")]
	public override void Test00_Valid_Input_Returns_Parsed_Result(string? input) =>
		Test00(input, Guid.Parse, M.ParseGuid, M.ParseGuid);

	[Theory]
	[InlineData("")]
	[InlineData("Invalid")]
	[InlineData("0")]
	public override void Test01_Invalid_Input_Returns_None(string? input) =>
		Test01(input, M.ParseGuid, M.ParseGuid);

	[Theory]
	[InlineData(null)]
	public override void Test02_Null_Input_Returns_None(string? input) =>
		Test02(input, M.ParseGuid, M.ParseGuid);
}
