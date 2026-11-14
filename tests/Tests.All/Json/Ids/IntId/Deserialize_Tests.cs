// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Ids;

namespace Wrap.Json.IntId_Tests;

public class Deserialize_Tests : Abstracts.Deserialize_Tests<Deserialize_Tests.TestIntId, int>
{
	[Theory]
	[MemberData(nameof(Helpers.Json.Valid_Numeric_Json_Data), MemberType = typeof(Helpers.Json))]
	public override void Test00_Deserialise__Valid_Json__Returns_Id_With_Value(string format)
	{
		Test00(format, Rnd.Int);
	}

	[Theory]
	[MemberData(nameof(Helpers.Json.Valid_Numeric_Json_Data), MemberType = typeof(Helpers.Json))]
	public override void Test01_Deserialise__Valid_Json__Returns_Object_With_Id_Value(string format)
	{
		Test01(format, Rnd.Int);
	}

	[Theory]
	[MemberData(nameof(Helpers.Json.Invalid_Json_Data), MemberType = typeof(Helpers.Json))]
	public override void Test02_Deserialise__Invalid_Json__Throws_JsonException(string input)
	{
		Test02(input);
	}

	[Theory]
	[MemberData(nameof(Helpers.Json.Invalid_Json_Data), MemberType = typeof(Helpers.Json))]
	public override void Test03_Deserialise__Invalid_Json_In_Object__Throws_JsonException(string input)
	{
		Test03(input);
	}

	public sealed record class TestIntId : IntId<TestIntId>;
}
