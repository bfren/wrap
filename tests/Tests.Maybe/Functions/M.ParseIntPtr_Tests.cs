// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.M_Tests;

public class ParseIntPtr_Tests : ParseInt_Tests<nint>
{
	protected override nint GetRndValue() =>
		Rnd.Ptr;

	public override void return_some_with_int_value__when_value_is_parseable_int() =>
		Test00(M.ParseIntPtr, M.ParseIntPtr);

	public override void return_none__when_value_is_null() =>
		Test01(M.ParseIntPtr, M.ParseIntPtr);

	public override void return_none__when_value_is_not_parseable_int(string? input) =>
		Test02(input, M.ParseIntPtr, M.ParseIntPtr);
}
