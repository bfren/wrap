// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Dapper.GuidIdTypeHandler_Tests;

public class Parse_Tests : Abstracts.Parse_Tests<Parse_Tests.TestGuidId, Guid, GuidIdTypeHandler<Parse_Tests.TestGuidId>>
{
	[Theory]
	[MemberData(nameof(Null_Or_Empty_Or_Invalid_Data))]
	public override void Test00_Null_Or_Empty_Or_Invalid_Input__Returns_Default(object input) =>
		Test00(input, Guid.Empty);

	[Theory]
	[InlineData("5da27e31-6f74-4256-a654-1075641324fe", null)]
	[InlineData(" 56f1fcf2-1fa4-4da4-9fc7-636afaf017f7 ", null)]
	[InlineData("582f6388075c4092a02e92842d9165a1", null)]
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
	public override void Test01_Valid_Input__Returns_Id(object input, Guid? expected) =>
		Test01(input, Guid.Parse);
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters

	public sealed record class TestGuidId : GuidId<TestGuidId>;
}
