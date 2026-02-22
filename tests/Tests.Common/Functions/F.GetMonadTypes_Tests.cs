// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.F_Tests;

public class GetMonadTypes_Tests
{
	[Fact]
	public void Type_Is_Not_Assignable_From_IMonad__Returns_Null()
	{
		// Arrange
		var type = typeof(GetMonadValueType_Tests);

		// Act
		var (monadType, valueType) = F.GetMonadTypes(type, typeof(IMonad<,>));

		// Assert
		Assert.Null(monadType);
		Assert.Null(valueType);
	}

	[Fact]
	public void Type_Does_Not_Implement_IMonad_With_Value_Type__Returns_Null()
	{
		// Arrange
		var type = typeof(TestWithoutValueType);

		// Act
		var (monadType, valueType) = F.GetMonadTypes(type, typeof(IMonad<,>));

		// Assert
		Assert.Null(monadType);
		Assert.Null(valueType);
	}

	[Fact]
	public void Type_Implements_Multiple_IMonad_Value_Types__Returns_Null()
	{
		// Arrange
		var type = typeof(TestWithMultipleValueTypes);

		// Act
		var (monadType, valueType) = F.GetMonadTypes(type, typeof(IMonad<,>));

		// Assert
		Assert.Null(monadType);
		Assert.Null(valueType);
	}

	[Fact]
	public void Type_Implements_IMonad_With_Value_Type_Once__Returns_Value_Type()
	{
		// Arrange
		var type = typeof(TestMonad);

		// Act
		var (monadType, valueType) = F.GetMonadTypes(type, typeof(IMonad<,>));

		// Assert
		Assert.Equal(typeof(TestMonad), monadType);
		Assert.Equal(typeof(DateTime), valueType);
	}

	public sealed record class TestWithoutValueType(object Value) : IMonad;

	public sealed record class TestWithMultipleValueTypes : IMonad<TestWithMultipleValueTypes, int>, IMonad<TestWithMultipleValueTypes, long>
	{
		public object Value { get; init; } = new();

		int IMonad<int>.Value
		{
			get => throw new NotImplementedException();
			init => throw new NotImplementedException();
		}

		long IMonad<long>.Value
		{
			get => throw new NotImplementedException();
			init => throw new NotImplementedException();
		}
	}

	public sealed record class TestMonad() : Monad<TestMonad, DateTime>(Rnd.DateTime);
}
