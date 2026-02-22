// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Ids;

namespace Wrap.Dapper.ULongIdTypeHandler_Tests;

public class SetValue_Tests : Abstracts.SetValue_Tests<SetValue_Tests.TestULongId, ulong, ULongIdTypeHandler<SetValue_Tests.TestULongId>>
{
	public override void Test00_Sets_Parameter__With_Correct_Type_And_Value() =>
		Test00(IdGen.ULongId<TestULongId>(), System.Data.DbType.UInt64);

	public sealed record class TestULongId : ULongId<TestULongId>;
}
