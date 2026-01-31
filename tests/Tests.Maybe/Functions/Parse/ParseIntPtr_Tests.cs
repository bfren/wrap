// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Functions_Tests;

public class ParseIntPtr_Tests : Abstracts.Parse_Tests<nint>
{
	public static TheoryData<string> Extreme_NInt_Input() =>
		[
			nint.MinValue.ToString(),
			nint.MaxValue.ToString()
		];

	[Theory]
	[MemberData(nameof(ParseInt16_Tests.Valid_Integer_Input), MemberType = typeof(ParseInt16_Tests))]
	[MemberData(nameof(Extreme_NInt_Input))]
	public override void Test00_Valid_Input_Returns_Parsed_Result(string? input) =>
		Test00(input, s => nint.Parse(s, F.DefaultCulture), M.ParseIntPtr, M.ParseIntPtr);

	[Theory]
	[MemberData(nameof(ParseInt16_Tests.Invalid_Integer_Input), MemberType = typeof(ParseInt16_Tests))]
	public override void Test01_Invalid_Input_Returns_None(string? input) =>
		Test01(input, M.ParseIntPtr, M.ParseIntPtr);

	[Theory]
	[InlineData(null)]
	public override void Test02_Null_Input_Returns_None(string? input) =>
		Test02(input, M.ParseIntPtr, M.ParseIntPtr);
}
