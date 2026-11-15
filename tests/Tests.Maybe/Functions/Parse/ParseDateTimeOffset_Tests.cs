// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Globalization;

namespace Wrap.Functions_Tests;

public class ParseDateTimeOffset_Tests : Abstracts.Parse_Tests<DateTimeOffset>
{
	[Theory]
	[MemberData(nameof(ParseDateTime_Tests.Valid_DateTime_Input), MemberType = typeof(ParseDateTime_Tests))]
	public override void Test00_Valid_Input_Returns_Parsed_Result(string? input) =>
		Test00(input, s => DateTimeOffset.Parse(s, F.DefaultCulture, DateTimeStyles.AssumeUniversal), M.ParseDateTimeOffset, M.ParseDateTimeOffset);

	[Theory]
	[MemberData(nameof(ParseDateTime_Tests.Invalid_DateTime_Input), MemberType = typeof(ParseDateTime_Tests))]
	public override void Test01_Invalid_Input_Returns_None(string? input) =>
		Test01(input, M.ParseDateTimeOffset, M.ParseDateTimeOffset);

	[Theory]
	[InlineData(null)]
	public override void Test02_Null_Input_Returns_None(string? input) =>
		Test02(input, M.ParseDateTimeOffset, M.ParseDateTimeOffset);

	[Theory]
	[InlineData("2026-07-01T10:00:00", 0)]
	[InlineData("2026-07-01", 0)]
	[InlineData("2026-07-01T10:00:00Z", 0)]
	[InlineData("2026-07-01T10:00:00+02:00", 2)]
	public void Offset__Is_Kept_Or_Assumed_Utc(string input, int expectedHours)
	{
		// Arrange
		var expected = TimeSpan.FromHours(expectedHours);

		// Act
		var r0 = M.ParseDateTimeOffset(input);
		var r1 = M.ParseDateTimeOffset(input.AsSpan());

		// Assert
		Assert.Equal(expected, r0.AssertSome().Offset);
		Assert.Equal(expected, r1.AssertSome().Offset);
	}
}
