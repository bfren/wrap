// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

public static partial class R
{
	/// <summary>
	/// Return an <see cref="Ok{T}"/> result holding <see langword="true"/>.
	/// </summary>
	/// <returns>An <see cref="Ok{T}"/> result holding <see langword="true"/>.</returns>
	public static Result<bool> Ok() =>
		Wrap(true);

	/// <summary>
	/// Wrap a value as a <see cref="Result{T}"/>.
	/// </summary>
	/// <remarks>
	/// A <see langword="null"/> value (including an empty <see cref="System.Nullable{T}"/>) gives a <see cref="Failure"/>,
	/// because the <see cref="Result{T}(Ok{T})"/> constructor turns a <see langword="null"/> <see cref="Ok{T}"/> into one.
	/// Use <c>Maybe&lt;T&gt;</c> for values that can be <see langword="null"/>.
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="value">Value to wrap.</param>
	/// <returns>
	/// An <see cref="Ok{T}"/> holding <paramref name="value"/>, or a <see cref="Failure"/> if <paramref name="value"/> is <see langword="null"/>.
	/// </returns>
	public static Result<T> Wrap<T>(T value) =>
		new Ok<T>(value);
}
