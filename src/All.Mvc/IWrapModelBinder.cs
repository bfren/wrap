// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Wrap.Mvc;

/// <summary>
/// Generic model binder for Wrap classes.
/// </summary>
/// <typeparam name="TValue">Value type.</typeparam>
public interface IWrapModelBinder<TValue> : IModelBinder
{
	/// <summary>
	/// Abstraction for getting value from binding value provider.
	/// </summary>
	/// <param name="provider">IValueProvider.</param>
	/// <param name="model">Model Name.</param>
	/// <returns>ValueProviderResult and ModelBindingResult.</returns>
	(ValueProviderResult valueResult, ModelBindingResult bindingResult) GetValue(IValueProvider provider, string model);

	/// <summary>
	/// Wrap a value to insert correctly into the binding context.
	/// </summary>
	/// <param name="value">Value to wrap.</param>
	/// <returns>Wrapped value</returns>
	object Wrap(TValue value);
}
