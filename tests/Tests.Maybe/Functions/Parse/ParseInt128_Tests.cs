// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Functions_Tests;

public class ParseInt128_Tests : Abstracts.Parse_Tests<Int128>
{
	public static TheoryData<string> Extreme_Long_Input() =>
		[
			Int128.MinValue.ToString(),
			Int128.MaxValue.ToString()
		];

	[Theory]
	[MemberData(nameof(ParseInt16_Tests.Valid_Integer_Input), MemberType = typeof(ParseInt16_Tests))]
	[MemberData(nameof(Extreme_Long_Input))]
	public override void Test00_Valid_Input_Returns_Parsed_Result(string? input) =>
		Test00(input, s => Int128.Parse(s, F.DefaultCulture), M.ParseInt128, M.ParseInt128);

	[Theory]
	[MemberData(nameof(ParseInt16_Tests.Invalid_Integer_Input), MemberType = typeof(ParseInt16_Tests))]
	public override void Test01_Invalid_Input_Returns_None(string? input) =>
		Test01(input, M.ParseInt128, M.ParseInt128);

	[Theory]
	[InlineData(null)]
	public override void Test02_Null_Input_Returns_None(string? input) =>
		Test02(input, M.ParseInt128, M.ParseInt128);
}
