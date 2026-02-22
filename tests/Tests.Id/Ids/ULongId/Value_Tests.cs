// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Ids.ULongId_Tests;

public class Value_Tests : Abstracts.Value_Tests
{
	[Fact]
	public override void Test00_Generic_Get__With_Value__Returns_Value() =>
		Test00(IdGen.ULongId<TestId>());

	[Fact]
	public override void Test01_Generic_Set__Receives_Correct_Type__Uses_Value() =>
		Test01<TestId, ulong>(Rnd.UInt64);

	public sealed record class TestId : ULongId<TestId>;
}
