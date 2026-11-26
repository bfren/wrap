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
	[InlineData("1,000", 1000)]
	public override void Test01_Valid_Input__Returns_Id(object input, int? expected) =>
		Test01(input, _ => (int)expected!);

	[Theory]
	[InlineData(3_000_000_000L)]
	[InlineData(-3_000_000_000L)]
	[InlineData("3000000000")]
	[InlineData("-3000000000")]
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

	[Fact]
	public void Overflow_Message_Uses_DefaultCulture_Not_CurrentCulture()
	{
		// Arrange
		var handler = new IntIdTypeHandler<TestIntId>();
		var current = System.Globalization.CultureInfo.CurrentCulture;
		System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("fr-FR");

		try
		{
			// Act
			var result = Record.Exception(() => handler.Parse(42.5));

			// Assert
			var ex = Assert.IsType<OverflowException>(result);
			Assert.StartsWith("Value '42.5'", ex.Message);
		}
		finally
		{
			System.Globalization.CultureInfo.CurrentCulture = current;
		}
	}

	[Theory]
	[MemberData(nameof(Whole_Decimal_Data))]
	public override void Test02_Whole_Decimal__Returns_Id(decimal input) =>
		Test02(input, 42);

	[Fact]
	public override void Test03_Decimal_Not_Whole__Throws_OverflowException() =>
		Test03();

	[Theory]
	[MemberData(nameof(Text_Number_Does_Not_Fit_Data))]
	public override void Test04_Text_Number_Does_Not_Fit__Throws_OverflowException(string input) =>
		Test04(input);

	[Theory]
	[MemberData(nameof(Whole_Text_Number_Data))]
	public override void Test05_Whole_Text_Number__Returns_Id(string input) =>
		Test05(input, 42);

	public sealed record class TestIntId : IntId<TestIntId>;
}
