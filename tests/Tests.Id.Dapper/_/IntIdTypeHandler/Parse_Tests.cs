// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Ids;

namespace Wrap.Dapper.IntIdTypeHandler_Tests;

public class Parse_Tests : Abstracts.Parse_Tests<Parse_Tests.TestIntId, int, IntIdTypeHandler<Parse_Tests.TestIntId>>
{
	[Theory]
	[MemberData(nameof(Null_Or_Empty_Or_Invalid_Data))]
	public override void Test00_Null_Or_Empty_Or_Invalid_Input__Returns_Default(object input) =>
		Test00(input, 0);

	[Theory]
	[InlineData(42, 42)]
	[InlineData(42.0, 42)]
	[InlineData("42", 42)]
	[InlineData(" 42 ", 42)]
	public override void Test01_Valid_Input__Returns_Id(object input, int? expected) =>
		Test01(input, _ => (int)expected!);

	public sealed record class TestIntId : IntId<TestIntId>;
}
