// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Wrap.Mvc.MonadModelBinder_Tests;

public abstract class MonadModelBinder_Tests<TValue>
{
	public (MonadModelBinder<TestMonad, TValue>, Vars) Setup(TValue value)
	{
		var provider = Substitute.For<IValueProvider>();

		var modelName = Rnd.Str;
		var valueProviderResult = value switch
		{
			TValue =>
				new(value.ToString()),

			_ =>
				ValueProviderResult.None
		};
		provider.GetValue(modelName).Returns(valueProviderResult);

		var context = Substitute.ForPartsOf<ModelBindingContext>();
		context.ValueProvider.Returns(provider);
		context.ModelName.Returns(modelName);

		var modelState = new ModelStateDictionary();
		context.ModelState.Returns(modelState);

		return (new(), new(
			context,
			modelName,
			provider,
			value
		));
	}

	public sealed record class Vars(
		ModelBindingContext Context,
		string ModelName,
		IValueProvider Provider,
		TValue Value
	);

	public record class TestMonad : Monad<TestMonad, TValue>;
}
