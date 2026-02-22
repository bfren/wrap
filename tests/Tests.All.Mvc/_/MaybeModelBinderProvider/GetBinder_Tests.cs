// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Mvc.MaybeModelBinderProvider_Tests;

public class GetBinder_Tests
{
	public class With_Maybe_Type : MaybeModelBinderProvider_Tests
	{
		[Fact]
		public void Returns_MaybeModelBinder()
		{
			// Arrange
			var (provider, v) = Setup(typeof(Maybe<string>));

			// Act
			var result = provider.GetBinder(v.Context);

			// Assert
			Assert.IsType<MaybeModelBinder<string>>(result);
		}
	}

	public class With_Other_Type : MaybeModelBinderProvider_Tests
	{
		[Fact]
		public void Returns_Null()
		{
			// Arrange
			var (provider, v) = Setup(typeof(GetBinder_Tests));

			// Act
			var result = provider.GetBinder(v.Context);

			// Assert
			Assert.Null(result);
		}
	}
}
