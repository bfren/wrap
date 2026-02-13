// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Ids;

namespace Wrap.Json.GuidId_Tests;

public class Deserialize_Tests : Abstracts.Deserialize_Tests<Deserialize_Tests.TestGuidId, Guid>
{
	[Theory]
	[MemberData(nameof(Helpers.Json.Valid_String_Json_Data), MemberType = typeof(Helpers.Json))]
	public override void Test00_Deserialise__Valid_Json__Returns_Id_With_Value(string format)
	{
		Test00(format, Rnd.Guid);
	}

	[Theory]
	[MemberData(nameof(Helpers.Json.Valid_String_Json_Data), MemberType = typeof(Helpers.Json))]
	public override void Test01_Deserialise__Valid_Json__Returns_Object_With_Id_Value(string format)
	{
		Test01(format, Rnd.Guid);
	}

	[Theory]
	[MemberData(nameof(Helpers.Json.Invalid_Json_Data), MemberType = typeof(Helpers.Json))]
	public override void Test02_Deserialise__Null_Or_Invalid_Json__Returns_Default_Id_Value(string input)
	{
		Test02(input, Guid.Empty);
	}

	[Theory]
	[MemberData(nameof(Helpers.Json.Invalid_Json_Data), MemberType = typeof(Helpers.Json))]
	public override void Test03_Deserialise__Null_Or_Invalid_Json__Returns_Object_With_Default_Id_Value(string input)
	{
		Test03(input, Guid.Empty);
	}

	public sealed record class TestGuidId : GuidId<TestGuidId>;
}
