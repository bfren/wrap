// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Ids;

namespace Wrap.Dapper.IntIdTypeHandler_Tests;

public class SetValue_Tests : Abstracts.SetValue_Tests<SetValue_Tests.TestIntId, int, IntIdTypeHandler<SetValue_Tests.TestIntId>>
{
	public override void Test00_Sets_Parameter__With_Correct_Type_And_Value() =>
		Test00(IdGen.IntId<TestIntId>(), System.Data.DbType.Int32);

	public sealed record class TestIntId : IntId<TestIntId>;
}
