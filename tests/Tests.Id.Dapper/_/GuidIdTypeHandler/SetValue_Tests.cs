// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Ids;

namespace Wrap.Dapper.GuidIdTypeHandler_Tests;

public class SetValue_Tests : Abstracts.SetValue_Tests<SetValue_Tests.TestGuidId, Guid, GuidIdTypeHandler<SetValue_Tests.TestGuidId>>
{
	public override void Test00_Sets_Parameter__With_Correct_Type_And_Value() =>
		Test00(IdGen.GuidId<TestGuidId>(), System.Data.DbType.Guid);

	public sealed record class TestGuidId : GuidId<TestGuidId>;
}
