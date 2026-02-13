// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Ids;

namespace Wrap.Json.UIntId_Tests;

public class Serialize_Tests : Abstracts.Serialize_Tests<Serialize_Tests.TestUIntId, uint>
{
	[Fact]
	public override void Test00_Serialize_Direct__Returns_Valid_Json()
	{
		Test00(Rnd.UInt32);
	}

	[Fact]
	public override void Test01_Serialize_Wrapped__Returns_Valid_Json()
	{
		Test01(Rnd.UInt32);
	}

	public sealed record class TestUIntId : UIntId<TestUIntId>;
}
