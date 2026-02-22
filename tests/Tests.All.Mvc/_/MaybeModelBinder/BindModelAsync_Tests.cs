// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Wrap.Mvc.MaybeModelBinder_Tests;

public class BindModelAsync_Tests
{
	public class With_None : MaybeModelBinder_Tests<string>
	{
		[Fact]
		public async Task Returns_None()
		{
			// Arrange
			var (binder, v) = Setup(null!);
			var expected = ModelBindingResult.Success((Maybe<string>)M.None);

			// Act
			await binder.BindModelAsync(v.Context);

			// Assert
			Assert.Null(v.Context.Model);
			Assert.Single(v.Context.ModelState.Values, x => x.AttemptedValue == nameof(M.None));
			Assert.Equal(expected, v.Context.Result);
		}
	}

	public class With_Value : MaybeModelBinder_Tests<Guid>
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
			var m = Assert.IsType<Some<Guid>>(v.Context.Result.Model);
			Assert.Equal(v.Value, m.Value);
		}
	}
}
