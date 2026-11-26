// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Wrap.Mvc;

/// <summary>
/// MVC model binder for <see cref="Maybe{T}"/>: a missing or empty value binds as <see cref="None"/>, and a value that
/// cannot be parsed fails binding.
/// </summary>
/// <typeparam name="T">Some value type.</typeparam>
public sealed class MaybeModelBinder<T> : WrapModelBinder<T>
{
	/// <summary>
	/// Return the result used when there is no value: binding succeeds with <see cref="None"/>.
	/// </summary>
	/// <remarks>
	/// The attempted value is empty, so a form shown again from ModelState leaves the field blank, and sending it
	/// back blank binds as <see cref="None"/> again.
	/// </remarks>
	/// <returns>A value provider result holding an empty string, and a successful binding result holding <see cref="None"/>.</returns>
	internal override (ValueProviderResult valueResult, ModelBindingResult bindingResult) Nothing() =>
		(
			new(string.Empty),
			ModelBindingResult.Success(default(Maybe<T>)) // don't use 'M.None' here to avoid incorrect compiler warnings
		);

	/// <summary>
	/// Wrap a parsed value as a <see cref="Maybe{T}"/>.
	/// </summary>
	/// <param name="value">Value to wrap.</param>
	/// <returns>A boxed <see cref="Maybe{T}"/>: <see cref="Some{T}"/> holding <paramref name="value"/>, or <see cref="None"/> if it is <see langword="null"/>.</returns>
	public override object Wrap(T value) =>
		M.Wrap(value);
}
