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
	where TMonad : IMonad<TMonad, TValue>, new()
{
	/// <summary>
	/// Parse Monad value from the binding context.
	/// </summary>
	/// <param name="bindingContext">ModelBindingContext.</param>
	public Task BindModelAsync(ModelBindingContext bindingContext)
	{
		// Perform bind
		var (valueResult, bindingResult) = GetValue(bindingContext.ValueProvider, bindingContext.ModelName);

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
	/// <param name="provider">IValueProvider.</param>
	/// <param name="model">Model Name.</param>
	/// <returns>ValueProviderResult and ModelBindingResult.</returns>
	internal (ValueProviderResult valueResult, ModelBindingResult? bindingResult) GetValue(IValueProvider provider, string model)
	{
		// Get the value from the context
		var valueProviderResult = provider.GetValue(model);
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
			return (valueProviderResult, JsonSerializer.Deserialize<TValue>($"\"{value}\"") switch
			{
				TValue x =>
					ModelBindingResult.Success(F.Wrap<TMonad, TValue>(x)),

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
