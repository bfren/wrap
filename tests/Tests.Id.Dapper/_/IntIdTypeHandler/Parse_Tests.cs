// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Ids;

namespace Wrap.Dapper.IntIdTypeHandler_Tests;

public class Parse_Tests : Abstracts.IntegerParse_Tests<Parse_Tests.TestIntId, int, IntIdTypeHandler<Parse_Tests.TestIntId>>
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

	[Theory]
	[InlineData(3_000_000_000L)]
	[InlineData(-3_000_000_000L)]
	[InlineData(42.5)]
	public void Number_Does_Not_Fit__Throws_OverflowException(object input)
	{
		// Arrange
		var handler = new IntIdTypeHandler<TestIntId>();

		// Act
		var result = Record.Exception(() => handler.Parse(input));

		// Assert
		Assert.IsType<OverflowException>(result);
	}

	[Theory]
	[MemberData(nameof(Whole_Decimal_Data))]
	public override void Test02_Whole_Decimal__Returns_Id(decimal input) =>
		Test02(input, 42);

	[Fact]
	public override void Test03_Decimal_Not_Whole__Throws_OverflowException() =>
		Test03();

	public sealed record class TestIntId : IntId<TestIntId>;
}
