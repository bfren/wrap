// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Wrap.Mvc;

/// <summary>
/// MVC model binder that reads a <typeparamref name="TValue"/> from the request and wraps it in a Wrap type.
/// </summary>
/// <typeparam name="TValue">Wrapped value type.</typeparam>
public interface IWrapModelBinder<TValue> : IModelBinder
{
	/// <summary>
	/// Read, parse and wrap the value named <paramref name="model"/> from <paramref name="provider"/>.
	/// </summary>
	/// <param name="provider">Value provider to read from.</param>
	/// <param name="model">Model name.</param>
	/// <returns>The raw value provider result, and the model binding result holding the wrapped value.</returns>
	(ValueProviderResult valueResult, ModelBindingResult bindingResult) GetValue(IValueProvider provider, string model);

	/// <summary>
	/// Wrap a parsed value in the type being bound.
	/// </summary>
	/// <param name="value">Value to wrap.</param>
	/// <returns>The wrapped value, boxed.</returns>
	object Wrap(TValue value);
}
