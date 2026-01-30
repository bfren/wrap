// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Functions_Tests;

public class ParseDouble_Tests : Abstracts.Parse_Tests<double>
{
	public static TheoryData<string> Extreme_Double_Input() =>
		[
			double.MinValue.ToString(),
			double.MaxValue.ToString()
		];

	[Theory]
	[MemberData(nameof(ParseSingle_Tests.Valid_Float_Input), MemberType = typeof(ParseSingle_Tests))]
	[MemberData(nameof(ParseSingle_Tests.Valid_Float_Exponential_Input), MemberType = typeof(ParseSingle_Tests))]
	[MemberData(nameof(Extreme_Double_Input))]
	public override void Test00_Valid_Input_Returns_Parsed_Result(string? input) =>
		Test00(input, s => double.Parse(s, F.DefaultCulture), M.ParseDouble, M.ParseDouble);

	[Theory]
	[MemberData(nameof(ParseSingle_Tests.Invalid_Float_Input), MemberType = typeof(ParseSingle_Tests))]
	public override void Test01_Invalid_Input_Returns_None(string? input) =>
		Test01(input, M.ParseDouble, M.ParseDouble);

	[Theory]
	[InlineData(null)]
	public override void Test02_Null_Input_Returns_None(string? input) =>
		Test02(input, M.ParseDouble, M.ParseDouble);
}
