// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Diagnostics.CodeAnalysis;

namespace Wrap;

/// <summary>
/// ID monad.
/// </summary>
/// <typeparam name="TId">Implementation type.</typeparam>
/// <typeparam name="TValue">ID value type.</typeparam>
public abstract record class Id<TId, TValue> : IId
	where TId : Id<TId, TValue>, new()
	where TValue : notnull
{
	/// <inheritdoc cref="IId.Value"/>
	public TValue Value { get; init; }

	object IId.Value =>
		Value;

	/// <summary>
	/// Internal implementations only.
	/// </summary>
	/// <param name="value">ID value.</param>
	private protected Id([DisallowNull] TValue value) =>
		Value = value;

	/// <summary>
	/// Create a new ID using the specified <paramref name="value"/>.
	/// </summary>
	/// <param name="value">ID value.</param>
	/// <returns>ID object.</returns>
#pragma warning disable CA1000 // Do not declare static members on generic types
	public static TId Create(TValue value) =>
		new() { Value = value };
#pragma warning restore CA1000 // Do not declare static members on generic types
}
