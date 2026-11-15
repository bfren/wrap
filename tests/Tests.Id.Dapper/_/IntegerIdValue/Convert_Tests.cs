// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

// the casts in the data are not redundant: they choose the boxed type, so each switch arm is tested
#pragma warning disable IDE0004

namespace Wrap.Dapper.IntegerIdValue_Tests;

public class Convert_Tests
{
	// fails the test if the string fallback is used, so direct conversion is proved
	private static Maybe<T> NoParse<T>(string? _) =>
		throw new InvalidOperationException("Parse should not be called.");

	public static TheoryData<object> Integer_Types_Data =>
		[
			(sbyte)42,
			(byte)42,
			(short)42,
			(ushort)42,
			42,
			42U,
			42L,
			42UL
		];

	public static TheoryData<object> Whole_Decimal_Data =>
		[
			42m,
			42.0m,
			42.00m
		];

	[Theory]
	[MemberData(nameof(Integer_Types_Data))]
	[MemberData(nameof(Whole_Decimal_Data))]
	public void Whole_Number__Converts_Without_Parse(object input)
	{
		// Arrange

		// Act
		var i = IntegerIdValue.Convert(input, NoParse<int>);
		var l = IntegerIdValue.Convert(input, NoParse<long>);
		var u = IntegerIdValue.Convert(input, NoParse<uint>);
		var ul = IntegerIdValue.Convert(input, NoParse<ulong>);

		// Assert
		i.AssertSome(42);
		l.AssertSome(42L);
		u.AssertSome(42U);
		ul.AssertSome(42UL);
	}

	public static TheoryData<object> Out_Of_Range_For_Int_Data =>
		[
			3_000_000_000L,
			-3_000_000_000L,
			(ulong)int.MaxValue + 1,
			(uint)int.MaxValue + 1,
			3_000_000_000m
		];

	[Theory]
	[MemberData(nameof(Out_Of_Range_For_Int_Data))]
	public void Out_Of_Range__Returns_None_Without_Parse(object input)
	{
		// Arrange

		// Act
		var result = IntegerIdValue.Convert(input, NoParse<int>);

		// Assert
		result.AssertNone();
	}

	public static TheoryData<object> Negative_Data =>
		[
			(sbyte)-1,
			(short)-1,
			-1,
			-1L,
			-1m
		];

	[Theory]
	[MemberData(nameof(Negative_Data))]
	public void Negative__Unsigned__Returns_None_Without_Parse(object input)
	{
		// Arrange

		// Act
		var u = IntegerIdValue.Convert(input, NoParse<uint>);
		var ul = IntegerIdValue.Convert(input, NoParse<ulong>);

		// Assert
		u.AssertNone();
		ul.AssertNone();
	}

	[Fact]
	public void Max_And_Min_Values__Convert()
	{
		// Arrange

		// Act
		var maxInt = IntegerIdValue.Convert((long)int.MaxValue, NoParse<int>);
		var minInt = IntegerIdValue.Convert((long)int.MinValue, NoParse<int>);
		var maxULong = IntegerIdValue.Convert(ulong.MaxValue, NoParse<ulong>);
		var maxULongDecimal = IntegerIdValue.Convert((decimal)ulong.MaxValue, NoParse<ulong>);

		// Assert
		maxInt.AssertSome(int.MaxValue);
		minInt.AssertSome(int.MinValue);
		maxULong.AssertSome(ulong.MaxValue);
		maxULongDecimal.AssertSome(ulong.MaxValue);
	}

	[Theory]
	[InlineData("42")]
	[InlineData(42.0)]
	[InlineData(42.5)]
	public void Other_Types__Use_Parse_With_String(object input)
	{
		// Arrange
		var parse = Substitute.For<Func<string?, Maybe<int>>>();

		// Act
		_ = IntegerIdValue.Convert(input, parse);

		// Assert
		parse.Received(1).Invoke(input.ToString());
	}

	[Fact]
	public void Decimal_Not_Whole__Uses_Parse()
	{
		// Arrange
		var input = 42.5m;
		var parse = Substitute.For<Func<string?, Maybe<int>>>();

		// Act
		_ = IntegerIdValue.Convert(input, parse);

		// Assert
		parse.Received(1).Invoke(input.ToString());
	}

	[Fact]
	public void Null__Uses_Parse_With_Null()
	{
		// Arrange
		var parse = Substitute.For<Func<string?, Maybe<int>>>();

		// Act
		_ = IntegerIdValue.Convert(null, parse);

		// Assert
		parse.Received(1).Invoke(null);
	}
}
