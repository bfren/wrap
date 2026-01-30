// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Functions_Tests;

public class ParseDateTimeOffset_Tests : Abstracts.Parse_Tests<DateTimeOffset>
{
	[Theory]
	[MemberData(nameof(ParseDateTime_Tests.Valid_DateTime_Input), MemberType = typeof(ParseDateTime_Tests))]
	public override void Test00_Valid_Input_Returns_Parsed_Result(string? input) =>
		Test00(input, s => DateTimeOffset.Parse(s, F.DefaultCulture), M.ParseDateTimeOffset, M.ParseDateTimeOffset);

	[Theory]
	[MemberData(nameof(ParseDateTime_Tests.Invalid_DateTime_Input), MemberType = typeof(ParseDateTime_Tests))]
	public override void Test01_Invalid_Input_Returns_None(string? input) =>
		Test01(input, M.ParseDateTimeOffset, M.ParseDateTimeOffset);

	[Theory]
	[InlineData(null)]
	public override void Test02_Null_Input_Returns_None(string? input) =>
		Test02(input, M.ParseDateTimeOffset, M.ParseDateTimeOffset);
}
