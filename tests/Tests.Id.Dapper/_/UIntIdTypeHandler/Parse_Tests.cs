// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Dapper.UIntIdTypeHandler_Tests;

public class Parse_Tests : Abstracts.Parse_Tests<Parse_Tests.TestUIntId, uint, UIntIdTypeHandler<Parse_Tests.TestUIntId>>
{
	[Theory]
	[MemberData(nameof(Null_Or_Empty_Or_Invalid_Data))]
	public override void Test00_Null_Or_Empty_Or_Invalid_Input__Returns_Default(object input) =>
		Test00(input, 0);

	[Theory]
	[InlineData(42, 42u)]
	[InlineData(42u, 42u)]
	[InlineData(42.0, 42u)]
	[InlineData("42", 42u)]
	[InlineData(" 42 ", 42u)]
	public override void Test01_Valid_Input__Returns_Id(object input, uint? expected) =>
		Test01(input, _ => (uint)expected!);

	public sealed record class TestUIntId : UIntId<TestUIntId>;
}
