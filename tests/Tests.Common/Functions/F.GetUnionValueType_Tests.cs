// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.F_Tests;

public class GetUnionValueType_Tests
{
	[Fact]
	public void Type_Is_Not_Assignable_From_IUnion__Returns_Null()
	{
		// Arrange
		var type = typeof(GetUnionValueType_Tests);

		// Act
		var result = F.GetUnionValueType(type, typeof(IUnion<,>));

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public void Type_Does_Not_Implement_IUnion_With_Value_Type__Returns_Null()
	{
		// Arrange
		var type = typeof(TestWithoutValueType);

		// Act
		var result = F.GetUnionValueType(type, typeof(IUnion<,>));

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public void Type_Implements_Multiple_IUnion_Value_Types__Returns_Null()
	{
		// Arrange
		var type = typeof(TestWithMultipleValueTypes);

		// Act
		var result = F.GetUnionValueType(type, typeof(IUnion<,>));

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public void Type_Implements_IUnion_With_Value_Type_Once__Returns_Value_Type()
	{
		// Arrange
		var type = typeof(TestUnion);

		// Act
		var result = F.GetUnionValueType(type, typeof(IUnion<,>));

		// Assert
		Assert.Equal(typeof(DateTime), result);
	}

	public sealed record class TestWithoutValueType(object Value) : IUnion;

	public sealed record class TestWithMultipleValueTypes : IUnion<TestWithMultipleValueTypes, int>, IUnion<TestWithMultipleValueTypes, long>
	{
		public object Value { get; init; } = new();

		int IUnion<int>.Value
		{
			get => throw new NotImplementedException();
			init => throw new NotImplementedException();
		}

		long IUnion<long>.Value
		{
			get => throw new NotImplementedException();
			init => throw new NotImplementedException();
		}
	}

	public sealed record class TestUnion() : Union<TestUnion, DateTime>(Rnd.DateTime);
}
