// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Dapper.UIntIdTypeHandler_Tests;

public class SetValue_Tests : Abstracts.SetValue_Tests<SetValue_Tests.TestUIntId, uint, UIntIdTypeHandler<SetValue_Tests.TestUIntId>>
{
	public override void Test00_Sets_Parameter__With_Correct_Type_And_Value() =>
		Test00(IdGen.UIntId<TestUIntId>(), System.Data.DbType.UInt32);

	public sealed record class TestUIntId : UIntId<TestUIntId>;
}
