// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Wrap.Mvc;

internal static class WrapModelBinderHelpers
{
	internal static JsonSerializerOptions Options { get; } =
		new() { NumberHandling = JsonNumberHandling.AllowReadingFromString };

}

/// <summary>
/// Generic model binder for Wrap classes.
/// </summary>
/// <typeparam name="T">Value type.</typeparam>
public abstract class WrapModelBinder<T> : IModelBinder
{
	/// <summary>
	/// Wrap a value to insert correctly into the binding context.
	/// </summary>
	/// <param name="value">Value to wrap.</param>
	/// <returns>Wrapped value</returns>
	internal abstract object Wrap(T value);

	/// <summary>
	/// Parse Monad value from the binding context.
	/// </summary>
	/// <param name="bindingContext">ModelBindingContext.</param>
	public virtual Task BindModelAsync(ModelBindingContext bindingContext)
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
		var valueResult = provider.GetValue(model);
		if (valueResult == ValueProviderResult.None)
		{
			return (ValueProviderResult.None, null);
		}

		// Ensure value is not null
		var value = valueResult.FirstValue;
		if (string.IsNullOrWhiteSpace(value))
		{
			return (ValueProviderResult.None, null);
		}

		// Attempt to parse the value using JSON serialiser
		try
		{
			return (valueResult, JsonSerializer.Deserialize<T>($"\"{value}\"", WrapModelBinderHelpers.Options) switch
			{
				T x =>
					ModelBindingResult.Success(Wrap(x)),

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
