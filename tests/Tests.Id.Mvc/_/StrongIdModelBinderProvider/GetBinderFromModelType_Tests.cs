// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Wrap.Exceptions;

using Wrap.Ids;

namespace Wrap.Mvc.StrongIdModelBinderProvider_Tests;

public class GetBinderFromModelType_Tests
{
	[Fact]
	public void ModelType_Does_Not_Implement_StrongId__Returns_Null()
	{
		// Arrange
		var type = typeof(RandomType);

		// Act
		var result = IdModelBinderProvider.GetBinderFromModelType(type);

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public void Unsupported_StrongId_Value_Type__Throws_ModelBinderException()
	{
		// Arrange
		var type = typeof(TestDateTimeId);

		// Act
		var action = () => IdModelBinderProvider.GetBinderFromModelType(type);

		// Assert
		Assert.Throws<ModelBinderException>(action);
	}

	[Fact]
	public void ModelType_Implements_GuidId__Returns_GuidIdModelBinder()
	{
		// Arrange
		var type = typeof(TestGuidId);

		// Act
		var result = IdModelBinderProvider.GetBinderFromModelType(type);

		// Assert
		Assert.IsType<IModelBinder>(result, false);
		Assert.IsType<IdModelBinder<TestGuidId, Guid>>(result, false);
	}

	[Fact]
	public void ModelType_Implements_IntId__Returns_IntIdModelBinder()
	{
		// Arrange
		var type = typeof(TestIntId);

		// Act
		var result = IdModelBinderProvider.GetBinderFromModelType(type);

		// Assert
		Assert.IsType<IModelBinder>(result, false);
		Assert.IsType<IdModelBinder<TestIntId, int>>(result, false);
	}

	[Fact]
	public void ModelType_Implements_LongId__Returns_LongIdModelBinder()
	{
		// Arrange
		var type = typeof(TestLongId);

		// Act
		var result = IdModelBinderProvider.GetBinderFromModelType(type);

		// Assert
		Assert.IsType<IModelBinder>(result, false);
		Assert.IsType<IdModelBinder<TestLongId, long>>(result, false);
	}

	public sealed record class RandomType;

	public sealed record class TestDateTimeId(DateTime Value) : Id<TestDateTimeId, DateTime>(Value)
	{
		public TestDateTimeId() : this(Rnd.DateTime) { }
	}

	public sealed record class TestGuidId : GuidId<TestGuidId>;

	public sealed record class TestIntId : IntId<TestIntId>;

	public sealed record class TestLongId : LongId<TestLongId>;
}
