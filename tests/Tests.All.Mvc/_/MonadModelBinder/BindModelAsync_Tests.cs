// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Wrap.Mvc.MonadModelBinder_Tests;

public class BindModelAsync_Tests
{
	public class With_None : MonadModelBinder_Tests<string>
	{
		[Fact]
		public async Task Fails()
		{
			// Arrange
			var (binder, v) = Setup(null!);
			var expected = ModelBindingResult.Failed();

			// Act
			await binder.BindModelAsync(v.Context);

			// Assert
			Assert.Null(v.Context.Model);
			Assert.Equal(expected, v.Context.Result);
			Assert.Equal(0, v.Context.ModelState.ErrorCount);
		}
	}

	public class With_Incorrect_Type : MonadModelBinder_Tests<Guid>
	{
		[Fact]
		public async Task Fails_And_Adds_ModelState_Error()
		{
			// Arrange
			var (binder, v) = Setup(Rnd.Guid);
			var incorrectValue = Rnd.Str;
			v.Provider.GetValue(v.ModelName).Returns(new ValueProviderResult(incorrectValue));

			// Act
			await binder.BindModelAsync(v.Context);

			// Assert
			Assert.Equal(ModelBindingResult.Failed(), v.Context.Result);
			var entry = v.Context.ModelState[v.ModelName];
			Assert.NotNull(entry);
			Assert.Equal(incorrectValue, entry.AttemptedValue);
			Assert.Single(entry.Errors);
		}
	}

	public class With_Value : MonadModelBinder_Tests<Guid>
	{
		[Fact]
		public async Task Sets_ModelState()
		{
			// Arrange
			var (binder, v) = Setup(Rnd.Guid);

			// Act
			await binder.BindModelAsync(v.Context);

			// Assert
			Assert.Single(v.Context.ModelState,
				x => v.Value.ToString() == x.Value?.RawValue?.ToString()
			);
		}

		[Fact]
		public async Task Sets_Result()
		{
			// Arrange
			var (binder, v) = Setup(Rnd.Guid);

			// Act
			await binder.BindModelAsync(v.Context);

			// Assert
			Assert.True(v.Context.Result.IsModelSet);
			var m = Assert.IsType<TestMonad>(v.Context.Result.Model);
			Assert.Equal(v.Value, m.Value);
		}
	}
}
