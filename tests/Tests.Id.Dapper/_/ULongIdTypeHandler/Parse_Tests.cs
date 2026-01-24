// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Dapper.ULongIdTypeHandler_Tests;

public class Parse_Tests : Abstracts.Parse_Tests<Parse_Tests.TestULongId, ulong, ULongIdTypeHandler<Parse_Tests.TestULongId>>
{
	[Theory]
	[MemberData(nameof(Null_Or_Empty_Or_Invalid_Data))]
	public override void Test00_Null_Or_Empty_Or_Invalid_Input__Returns_Default(object input) =>
		Test00(input, 0);

	[Theory]
	[InlineData(42, 42UL)]
	[InlineData(42L, 42UL)]
	[InlineData(42UL, 42UL)]
	[InlineData(42.0, 42UL)]
	[InlineData("42", 42UL)]
	[InlineData(" 42 ", 42UL)]
	public override void Test01_Valid_Input__Returns_Id(object input, ulong? expected) =>
		Test01(input, _ => (ulong)expected!);

	public sealed record class TestULongId : ULongId<TestULongId>;
}
