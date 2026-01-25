// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Dapper;

namespace Abstracts;

public abstract class Parse_Tests<TId, TIdValue, TIdTypeHandler>
	where TId : Id<TId, TIdValue>, new()
	where TIdValue : struct
	where TIdTypeHandler : IdTypeHandler<TId, TIdValue>, new()
{
	public static TheoryData<object> Null_Or_Empty_Or_Invalid_Data =>
		[
			null!,
			"",
			" ",
			true,
			"something wrong here"
		];

	public abstract void Test00_Null_Or_Empty_Or_Invalid_Input__Returns_Default(object input);

	protected static void Test00(object input, TIdValue defaultValue)
	{
		// Arrange
		var handler = new TIdTypeHandler();

		// Act
		var result = handler.Parse(input);

		// Assert
		Assert.Equal(defaultValue, result?.Value);
	}

	public abstract void Test01_Valid_Input__Returns_Id(object input, TIdValue? expected);

	protected static void Test01(object input, Func<string, TIdValue> getExpected)
	{
		// Arrange
		var handler = new TIdTypeHandler();
		var expected = getExpected(input.ToString()!);

		// Act
		var result = handler.Parse(input);

		// Assert
		Assert.Equal(expected, result?.Value);
	}
}
