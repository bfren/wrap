// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.M_Tests;

public class ParseInt16_Tests : ParseInt_Tests<short>
{
	protected override short GetRndValue() =>
		Rnd.Sht;

	public override void return_some_with_int_value__when_value_is_parseable_int() =>
		Test00(M.ParseInt16, M.ParseInt16);

	public override void return_none__when_value_is_null() =>
		Test01(M.ParseInt16, M.ParseInt16);

	public override void return_none__when_value_is_not_parseable_int(string? input) =>
		Test02(input, M.ParseInt16, M.ParseInt16);
}
