// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Functions_Tests;

public class ParseUIntPtr_Tests : Abstracts.Parse_Tests<nuint>
{
	public static TheoryData<string> Extreme_NUInt_Input() =>
		[
			nuint.MinValue.ToString(),
			nuint.MaxValue.ToString()
		];

	[Theory]
	[MemberData(nameof(ParseUInt16_Tests.Valid_Unsigned_Integer_Input), MemberType = typeof(ParseUInt16_Tests))]
	[MemberData(nameof(Extreme_NUInt_Input))]
	public override void Test00_Valid_Input_Returns_Parsed_Result(string? input) =>
		Test00(input, s => nuint.Parse(s, F.DefaultCulture), M.ParseUIntPtr, M.ParseUIntPtr);

	[Theory]
	[MemberData(nameof(ParseUInt16_Tests.Invalid_Unsigned_Integer_Input), MemberType = typeof(ParseUInt16_Tests))]
	public override void Test01_Invalid_Input_Returns_None(string? input) =>
		Test01(input, M.ParseUIntPtr, M.ParseUIntPtr);

	[Theory]
	[InlineData(null)]
	public override void Test02_Null_Input_Returns_None(string? input) =>
		Test02(input, M.ParseUIntPtr, M.ParseUIntPtr);
}
