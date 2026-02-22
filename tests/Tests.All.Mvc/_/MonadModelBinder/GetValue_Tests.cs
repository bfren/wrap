// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;

namespace Wrap.Mvc.MonadModelBinder_Tests;

public class GetValue_Tests
{
	public class With_None : MonadModelBinder_Tests<string>
	{
		[Fact]
		public void Returns_None()
		{
			// Arrange
			var (binder, v) = Setup(null!);

			// Act
			var (valueResult, bindingResult) = binder.GetValue(v.Provider, v.ModelName);

			// Assert
			Assert.Equal(ValueProviderResult.None, valueResult);
			Assert.False(bindingResult.IsModelSet);
		}
	}

	public class With_Null : MonadModelBinder_Tests<string>
	{
		[Fact]
		public void Returns_None()
		{
			// Arrange
			var (binder, v) = Setup(null!);
			var nullResult = new ValueProviderResult(new StringValues(value: null));
			v.Provider.GetValue(v.ModelName).Returns(nullResult);

			// Act
			var (valueResult, bindingResult) = binder.GetValue(v.Provider, v.ModelName);

			// Assert
			Assert.Equal(ValueProviderResult.None, valueResult);
			Assert.False(bindingResult.IsModelSet);
		}
	}

	public class With_Empty : MonadModelBinder_Tests<string>
	{
		[Theory]
		[InlineData("")]
		[InlineData(" ")]
		public void Returns_None(string input)
		{
			// Arrange
			var (binder, v) = Setup(input);

			// Act
			var (valueResult, bindingResult) = binder.GetValue(v.Provider, v.ModelName);

			// Assert
			Assert.Equal(ValueProviderResult.None, valueResult);
			Assert.False(bindingResult.IsModelSet);
		}
	}

	public class With_Value
	{
		public class Incorrect_Type : MonadModelBinder_Tests<Guid>
		{
			[Fact]
			public void Returns_None()
			{
				// Arrange
				var (binder, v) = Setup(Rnd.Guid);
				var incorrectValue = new ValueProviderResult(Rnd.Str);
				v.Provider.GetValue(v.ModelName).Returns(incorrectValue);

				// Act
				var (valueResult, bindingResult) = binder.GetValue(v.Provider, v.ModelName);

				// Assert
				Assert.Equal(ValueProviderResult.None, valueResult);
				Assert.False(bindingResult.IsModelSet);
			}
		}

		public class Correct_Type : MonadModelBinder_Tests<long>
		{
			[Fact]
			public void Returns_Model_Value()
			{
				// Arrange
				var (binder, v) = Setup(Rnd.Lng);

				// Act
				var (valueResult, bindingResult) = binder.GetValue(v.Provider, v.ModelName);

				// Assert
				Assert.Equal(v.Value.ToString(), valueResult.FirstValue);
				Assert.True(bindingResult.IsModelSet);
				var m = Assert.IsType<TestMonad>(bindingResult.Model);
				Assert.Equal(v.Value, m.Value);
			}
		}
	}
}
