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
#pragma warning disable IDE0004 // false positive: Success takes object, so without the cast a bare None is boxed instead of Maybe<T>
	internal override (ValueProviderResult valueResult, ModelBindingResult bindingResult) Nothing() =>
		(new(nameof(M.None)), ModelBindingResult.Success((Maybe<T>)M.None));
#pragma warning restore IDE0004

	/// <inheritdoc/>
	public override object Wrap(T value) =>
		M.Wrap(value);
}
