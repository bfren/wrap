// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Mvc;

namespace Abstracts;

public abstract class Parse_Tests<TBinder, TId, TIdValue>
	where TBinder : IdModelBinder<TId, TIdValue>, new()
	where TId : Id<TId, TIdValue>, new()
	where TIdValue : struct
{
	public abstract void Test00_Valid_Input_Returns_Parsed_Result(string? input);

	protected static void Test00(string? input, Func<string, TIdValue> getExpected)
	{
		// Arrange
		var expected = getExpected(input ?? string.Empty);
		var binder = new TBinder();

		// Act
		var result = binder.Parse(input);

		// Assert
		var some = result.AssertSome();
		Assert.Equal(expected, some);
	}

	public abstract void Test01_Invalid_Input_Returns_None(string? input);

	protected static void Test01(string? input)
	{
		// Arrange
		var binder = new TBinder();

		// Act
		var result = binder.Parse(input);

		// Assert
		result.AssertNone();
	}

	public abstract void Test02_Null_Input_Returns_None(string? input);

	protected static void Test02(string? input)
	{
		// Arrange
		var binder = new TBinder();

		// Act
		var result = binder.Parse(input);

		// Assert
		result.AssertNone();
	}
}
