// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Dapper;

namespace Abstracts;

public abstract class IntegerParse_Tests<TId, TIdValue, TIdTypeHandler> : Parse_Tests<TId, TIdValue, TIdTypeHandler>
	where TId : Id<TId, TIdValue>, new()
	where TIdValue : struct
	where TIdTypeHandler : IdTypeHandler<TId, TIdValue>, new()
{
	public static TheoryData<decimal> Whole_Decimal_Data =>
		[
			42m,
			42.0m,
			42.00m
		];

	public abstract void Test02_Whole_Decimal__Returns_Id(decimal input);

	protected static void Test02(decimal input, TIdValue expected)
	{
		// Arrange
		var handler = new TIdTypeHandler();

		// Act
		var result = handler.Parse(input);

		// Assert
		Assert.Equal(expected, result?.Value);
	}

	public abstract void Test03_Decimal_Not_Whole__Throws_OverflowException();

	protected static void Test03()
	{
		// Arrange
		var handler = new TIdTypeHandler();

		// Act
		var result = Record.Exception(() => handler.Parse(42.5m));

		// Assert
		var ex = Assert.IsType<OverflowException>(result);
		Assert.Contains("is not a whole number that fits in", ex.Message);
	}

	public static TheoryData<string> Whole_Text_Number_Data =>
		[
			"42.0",
			" 42.00 ",
			"4.2E1"
		];

	public abstract void Test05_Whole_Text_Number__Returns_Id(string input);

	protected static void Test05(string input, TIdValue expected)
	{
		// Arrange
		var handler = new TIdTypeHandler();

		// Act
		var result = handler.Parse(input);

		// Assert
		Assert.Equal(expected, result?.Value);
	}

	public static TheoryData<string> Text_Number_Does_Not_Fit_Data =>
		[
			"42.5",
			" 42.5 ",
			"1e30",
			"-1e30"
		];

	public abstract void Test04_Text_Number_Does_Not_Fit__Throws_OverflowException(string input);

	protected static void Test04(string input)
	{
		// Arrange
		var handler = new TIdTypeHandler();

		// Act
		var result = Record.Exception(() => handler.Parse(input));

		// Assert
		var ex = Assert.IsType<OverflowException>(result);
		Assert.Contains("is not a whole number that fits in", ex.Message);
	}
}
