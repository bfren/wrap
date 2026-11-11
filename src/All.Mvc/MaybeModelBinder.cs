// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Wrap.Mvc;

/// <summary>
/// <see cref="Maybe{T}"/> MVC model binder.
/// </summary>
/// <typeparam name="T">Some value type.</typeparam>
public sealed class MaybeModelBinder<T> : WrapModelBinder<T>
{
	/// <inheritdoc/>
	internal override (ValueProviderResult valueResult, ModelBindingResult bindingResult) Nothing() =>
		// don't use 'M.None' here to avoid incorrect compiler warnings
		(new(nameof(M.None)), ModelBindingResult.Success(default(Maybe<T>)));

	/// <inheritdoc/>
	public override object Wrap(T value) =>
		M.Wrap(value);
}
