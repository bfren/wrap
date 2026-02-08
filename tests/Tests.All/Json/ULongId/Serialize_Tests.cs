// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Ids;

namespace Wrap.Json.ULongId_Tests;

public class Serialize_Tests : Abstracts.Serialize_Tests<Serialize_Tests.TestULongId, ulong>
{
	[Fact]
	public override void Test00_Serialize_Direct__Returns_Valid_Json()
	{
		Test00(Rnd.UInt64);
	}

	[Fact]
	public override void Test01_Serialize_Wrapped__Returns_Valid_Json()
	{
		Test01(Rnd.UInt64);
	}

	public sealed record class TestULongId : ULongId<TestULongId>;
}
