// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Ids;

namespace Wrap.Json.IntId_Tests;

public class Serialize_Tests : Abstracts.Serialize_Tests<Serialize_Tests.TestIntId, int>
{
	[Fact]
	public override void Test00_Serialize_Direct__Returns_Valid_Json()
	{
		Test00(Rnd.Int);
	}

	[Fact]
	public override void Test01_Serialize_Wrapped__Returns_Valid_Json()
	{
		Test01(Rnd.Int);
	}

	public sealed record class TestIntId : IntId<TestIntId>;
}
