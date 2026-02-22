// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Ids;

namespace Wrap.Json.UIntId_Tests;

public class Deserialize_Tests : Abstracts.Deserialize_Tests<Deserialize_Tests.TestUIntId, uint>
{
	[Theory]
	[MemberData(nameof(Helpers.Json.Valid_Numeric_Json_Data), MemberType = typeof(Helpers.Json))]
	public override void Test00_Deserialise__Valid_Json__Returns_Id_With_Value(string format)
	{
		Test00(format, Rnd.UInt32);
	}

	[Theory]
	[MemberData(nameof(Helpers.Json.Valid_Numeric_Json_Data), MemberType = typeof(Helpers.Json))]
	public override void Test01_Deserialise__Valid_Json__Returns_Object_With_Id_Value(string format)
	{
		Test01(format, Rnd.UInt32);
	}

	[Theory]
	[MemberData(nameof(Helpers.Json.Invalid_Json_Data), MemberType = typeof(Helpers.Json))]
	public override void Test02_Deserialise__Null_Or_Invalid_Json__Returns_Default_Id_Value(string input)
	{
		Test02(input, 0);
	}

	[Theory]
	[MemberData(nameof(Helpers.Json.Invalid_Json_Data), MemberType = typeof(Helpers.Json))]
	public override void Test03_Deserialise__Null_Or_Invalid_Json__Returns_Object_With_Default_Id_Value(string input)
	{
		Test03(input, 0);
	}

	public sealed record class TestUIntId : UIntId<TestUIntId>;
}
