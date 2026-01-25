// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Extensions;

namespace Wrap;

/// <summary>
/// Unsafe wrapper to enable unsafe (i.e. may result in null values) functions.
/// </summary>
/// <typeparam name="T">Ok value type.</typeparam>
public readonly record struct Unsafe<T>
{
	/// <summary>
	/// Wrapped <see cref="Result{T}"/> object.
	/// </summary>
	internal Result<T> Result { get; init; }

	/// <summary>
	/// Internal creation only (via <see cref="ResultExtensions.Unsafe{T}(Result{T})"/>).
	/// </summary>
	/// <param name="result"><see cref="Result{T}"/> object to wrap.</param>
	internal Unsafe(Result<T> result) =>
		Result = result;
}
