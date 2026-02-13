// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Wrap.Mvc;

/// <summary>
/// <see cref="Monad{TMonad, TValue}"/> MVC model binder.
/// </summary>
/// <typeparam name="TMonad">Monad type.</typeparam>
/// <typeparam name="TValue">Value type.</typeparam>
public sealed class MonadModelBinder<TMonad, TValue> : IModelBinder
	where TMonad : Monad<TMonad, TValue>, new()
{
	/// <summary>
	/// Parse Monad value from the binding context.
	/// </summary>
	/// <param name="bindingContext">ModelBindingContext.</param>
	public Task BindModelAsync(ModelBindingContext bindingContext)
	{
		// Perform bind
		var (valueResult, bindingResult) = BindModel(bindingContext.ValueProvider, bindingContext.ModelName);

		// Set binding values
		if (valueResult != ValueProviderResult.None)
		{
			// Set the model state value
			bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueResult);

			// Set the binding result
			bindingContext.Result = bindingResult ?? ModelBindingResult.Failed();
		}

		return Task.CompletedTask;
	}

	/// <summary>
	/// Binding abstraction to enable testing.
	/// </summary>
	/// <param name="valueProvider">IValueProvider.</param>
	/// <param name="modelName">Model Name.</param>
	/// <returns>ValueProviderResult and ModelBindingResult.</returns>
	internal (ValueProviderResult, ModelBindingResult?) BindModel(IValueProvider valueProvider, string modelName)
	{
		// Get the value from the context
		var valueProviderResult = valueProvider.GetValue(modelName);
		if (valueProviderResult == ValueProviderResult.None)
		{
			return (ValueProviderResult.None, null);
		}

		// Ensure value is not null
		var value = valueProviderResult.FirstValue;
		if (value is null)
		{
			return (ValueProviderResult.None, null);
		}

		// Attempt to parse the value using JSON serialiser
		try
		{
			return (valueProviderResult, JsonSerializer.Deserialize<TValue>(value) switch
			{
				TValue x =>
					ModelBindingResult.Success(F.Wrap<TMonad, TValue>(x)),

				_ when typeof(TValue).IsValueType =>
					ModelBindingResult.Success(F.Wrap<TMonad, TValue>(default)),

				_ =>
					null
			});
		}
		catch
		{
			return (ValueProviderResult.None, null);
		}
	}
}
