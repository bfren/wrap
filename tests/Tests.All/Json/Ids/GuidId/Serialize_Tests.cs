// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Ids;

namespace Wrap.Json.GuidId_Tests;

public class Serialize_Tests : Abstracts.Serialize_Tests<Serialize_Tests.TestGuidId, Guid>
{
	[Fact]
	public override void Test00_Serialize_Direct__Returns_Valid_Json()
	{
		Test00(Rnd.Guid);
	}

	[Fact]
	public override void Test01_Serialize_Wrapped__Returns_Valid_Json()
	{
		Test01(Rnd.Guid);
	}

	public sealed record class TestGuidId : GuidId<TestGuidId>;
}
