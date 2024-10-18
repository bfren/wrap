// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.M_Tests;

public class ParseUInt16_Tests : ParseInt_Tests<ushort>
{
	protected override ushort GetRndValue() =>
		Rnd.USht;

	public override void return_some_with_int_value__when_value_is_parseable_int() =>
		Test00(M.ParseUInt16, M.ParseUInt16);

	public override void return_none__when_value_is_null() =>
		Test01(M.ParseUInt16, M.ParseUInt16);

	public override void return_none__when_value_is_not_parseable_int(string? input) =>
		Test02(input, M.ParseUInt16, M.ParseUInt16);
}
