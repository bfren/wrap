// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Ids;

namespace Wrap.Dapper.LongIdTypeHandler_Tests;

public class Parse_Tests : Abstracts.Parse_Tests<Parse_Tests.TestLongId, long, LongIdTypeHandler<Parse_Tests.TestLongId>>
{
	[Theory]
	[MemberData(nameof(Null_Or_Empty_Or_Invalid_Data))]
	public override void Test00_Null_Or_Empty_Or_Invalid_Input__Returns_Default(object input) =>
		Test00(input, 0);

	[Theory]
	[InlineData(42, 42L)]
	[InlineData(42L, 42L)]
	[InlineData(42.0, 42L)]
	[InlineData("42", 42L)]
	[InlineData(" 42 ", 42L)]
	public override void Test01_Valid_Input__Returns_Id(object input, long? expected) =>
		Test01(input, _ => (long)expected!);

	public sealed record class TestLongId : LongId<TestLongId>;
}
