// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Diagnostics.CodeAnalysis;

namespace Wrap;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TValue"></typeparam>
public abstract record class Id<TValue> : IId
	where TValue : notnull
{
	/// <inheritdoc cref="IId.Value"/>
	public TValue Value { get; init; }

	object IId.Value =>
		Value;

	/// <summary>
	/// Internal implementations only
	/// </summary>
	/// <param name="value">ID Value</param>
	private protected Id([DisallowNull] TValue value) =>
		Value = value;
}
