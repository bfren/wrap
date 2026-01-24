// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.I_Tests;

public class GetIdValueType_Tests
{
	[Fact]
	public void Type_Is_Not_Assignable_From_IId__Returns_Null()
	{
		// Arrange
		var type = typeof(GetIdValueType_Tests);

		// Act
		var result = I.GetIdValueType(type);

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public void Type_Does_Not_Implement_IId_With_Value_Type__Returns_Null()
	{
		// Arrange
		var type = typeof(TestIdWithoutValueType);

		// Act
		var result = I.GetIdValueType(type);

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public void Type_Implements_Multiple_IId_Value_Types__Returns_Null()
	{
		// Arrange
		var type = typeof(TestIdWithMultipleValueTypes);

		// Act
		var result = I.GetIdValueType(type);

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public void Type_Implements_IId_With_Value_Type_Once__Returns_Value_Type()
	{
		// Arrange
		var type = typeof(TestId);

		// Act
		var result = I.GetIdValueType(type);

		// Assert
		Assert.Equal(typeof(DateTime), result);
	}

	public sealed record class TestIdWithoutValueType(object Value) : IUnion;

	public sealed record class TestIdWithMultipleValueTypes : IId<TestIdWithMultipleValueTypes, int>, IId<TestIdWithMultipleValueTypes, long>
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

	public sealed record class TestId() : Id<TestId, DateTime>(Rnd.DateTime);
}
