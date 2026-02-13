// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Mvc.MonadModelBinderProvider_Tests;

public class GetBinder_Tests
{
	public class With_Monad_Type : MonadModelBinderProvider_Tests
	{
		[Fact]
		public void Returns_MonadModelBinder()
		{
			// Arrange
			var (provider, v) = Setup(typeof(Postcode));

			// Act
			var result = provider.GetBinder(v.Context);

			// Assert
			Assert.IsType<MonadModelBinder<Postcode, string>>(result);
		}
	}

	public class With_Other_Type : MonadModelBinderProvider_Tests
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

	public record class Postcode : Monad<Postcode, string>;
}
