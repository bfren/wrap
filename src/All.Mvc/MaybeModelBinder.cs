// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Wrap.Mvc;

/// <summary>
/// MVC model binder for <see cref="Maybe{T}"/>: a missing, empty or invalid value binds as <see cref="None"/>.
/// </summary>
/// <typeparam name="T">Some value type.</typeparam>
public sealed class MaybeModelBinder<T> : WrapModelBinder<T>
{
	/// <summary>
	/// Return the result used when there is no valid value: binding succeeds with <see cref="None"/>.
	/// </summary>
	/// <returns>A value provider result holding <c>"None"</c>, and a successful binding result holding <see cref="None"/>.</returns>
	internal override (ValueProviderResult valueResult, ModelBindingResult bindingResult) Nothing() =>
		// don't use 'M.None' here to avoid incorrect compiler warnings
		(new(nameof(M.None)), ModelBindingResult.Success(default(Maybe<T>)));

	/// <summary>
	/// Wrap a parsed value as a <see cref="Maybe{T}"/>.
	/// </summary>
	/// <param name="value">Value to wrap.</param>
	/// <returns>A boxed <see cref="Maybe{T}"/>: <see cref="Some{T}"/> holding <paramref name="value"/>, or <see cref="None"/> if it is <see langword="null"/>.</returns>
	public override object Wrap(T value) =>
		M.Wrap(value);
}
