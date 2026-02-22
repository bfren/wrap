// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Functions_Tests;

public class ParseUInt128_Tests : Abstracts.Parse_Tests<UInt128>
{
	public static TheoryData<string> Extreme_UInt128_Input() =>
		[
			UInt128.MinValue.ToString(),
			UInt128.MaxValue.ToString()
		];

	[Theory]
	[MemberData(nameof(ParseUInt16_Tests.Valid_Unsigned_Integer_Input), MemberType = typeof(ParseUInt16_Tests))]
	[MemberData(nameof(Extreme_UInt128_Input))]
	public override void Test00_Valid_Input_Returns_Parsed_Result(string? input) =>
		Test00(input, s => UInt128.Parse(s, F.DefaultCulture), M.ParseUInt128, M.ParseUInt128);

	[Theory]
	[MemberData(nameof(ParseUInt16_Tests.Invalid_Unsigned_Integer_Input), MemberType = typeof(ParseUInt16_Tests))]
	public override void Test01_Invalid_Input_Returns_None(string? input) =>
		Test01(input, M.ParseUInt128, M.ParseUInt128);

	[Theory]
	[InlineData(null)]
	public override void Test02_Null_Input_Returns_None(string? input) =>
		Test02(input, M.ParseUInt128, M.ParseUInt128);
}
