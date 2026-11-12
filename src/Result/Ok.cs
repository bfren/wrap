// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// The 'Ok' case of <see cref="Result{T}"/>, which holds the value of a successful operation (see <seealso cref="Failure"/>).
/// </summary>
/// <remarks>
/// An <see cref="Ok{T}"/> holding <see langword="null"/> becomes a <see cref="Failure"/> when it is
/// converted to <see cref="Result{T}"/>, so <see cref="Value"/> is never <see langword="null"/> when you
/// match it from a <see cref="Result{T}"/>.
/// </remarks>
/// <typeparam name="T">Ok value type.</typeparam>
/// <param name="Value">Ok value.</param>
public readonly record struct Ok<T>(T Value);
