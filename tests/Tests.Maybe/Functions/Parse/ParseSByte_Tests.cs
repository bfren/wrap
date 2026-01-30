// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Functions_Tests;

public class ParseSByte_Tests : Abstracts.Parse_Tests<sbyte>
{
	[Theory]
	[MemberData(nameof(ParseByte_Tests.Valid_Byte_Input), MemberType = typeof(ParseByte_Tests))]
	[MemberData(nameof(ParseByte_Tests.Negative_Byte_Input), MemberType = typeof(ParseByte_Tests))]
	public override void Test00_Valid_Input_Returns_Parsed_Result(string? input) =>
		Test00(input, s => sbyte.Parse(s, F.DefaultCulture), M.ParseSByte, M.ParseSByte);

	[Theory]
	[MemberData(nameof(ParseByte_Tests.Invalid_Byte_Input), MemberType = typeof(ParseByte_Tests))]
	public override void Test01_Invalid_Input_Returns_None(string? input) =>
		Test01(input, M.ParseSByte, M.ParseSByte);

	[Theory]
	[InlineData(null)]
	public override void Test02_Null_Input_Returns_None(string? input) =>
		Test02(input, M.ParseSByte, M.ParseSByte);
}
