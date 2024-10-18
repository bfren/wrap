// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.M_Tests;

public class ParseInt64_Tests : ParseInt_Tests<long>
{
	protected override long GetRndValue() =>
		Rnd.Lng;

	public override void return_some_with_int_value__when_value_is_parseable_int() =>
		Test00(M.ParseInt64, M.ParseInt64);

	public override void return_none__when_value_is_null() =>
		Test01(M.ParseInt64, M.ParseInt64);

	public override void return_none__when_value_is_not_parseable_int(string? input) =>
		Test02(input, M.ParseInt64, M.ParseInt64);
}
