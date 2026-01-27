// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Ids;

namespace Wrap.Dapper.LongIdTypeHandler_Tests;

public class SetValue_Tests : Abstracts.SetValue_Tests<SetValue_Tests.TestLongId, long, LongIdTypeHandler<SetValue_Tests.TestLongId>>
{
	public override void Test00_Sets_Parameter__With_Correct_Type_And_Value() =>
		Test00(IdGen.LongId<TestLongId>(), System.Data.DbType.Int64);

	public sealed record class TestLongId : LongId<TestLongId>;
}
